namespace Infrastructure.Core.Implementations

open System
open System.Data
open Npgsql
open Dapper
open Dapper.FSharp
open Infrastructure.Core.Types
open Infrastructure.Core.Exceptions

module PostgreSQL =
    module private TypeHandlers =
        type TimeOnlyHandler() =
            inherit SqlMapper.TypeHandler<TimeOnly>()

            override _.SetValue(param, value) =
                (param :?> NpgsqlParameter).Value <- value

            override _.Parse(value) =
                match value with
                | :? TimeSpan as ts -> TimeOnly(ts.Hours, ts.Minutes, ts.Seconds)
                | :? DateTime as dt -> TimeOnly(dt.Hour, dt.Minute, dt.Second)
                | :? TimeOnly as _to -> _to
                | _ -> failwith $"Unexpected value type for TimeOnly: {value.GetType()}"

        type TimeOnlyOptionHandler() =
            inherit SqlMapper.TypeHandler<TimeOnly option>()

            override _.SetValue(param, _option) =
                (param :?> NpgsqlParameter).Value <- _option

            override _.Parse(value) =
                match value with
                | :? TimeSpan as ts -> Some(TimeOnly(ts.Hours, ts.Minutes, ts.Seconds))
                | :? DateTime as dt -> Some(TimeOnly(dt.Hour, dt.Minute, dt.Second))
                | :? TimeOnly as _to -> Some(_to)
                | _ -> failwith $"Unexpected value type for TimeOnly option: {value.GetType()}"

        type DateOnlyHandler() =
            inherit SqlMapper.TypeHandler<DateOnly>()

            override _.SetValue(param, value) =
                (param :?> NpgsqlParameter).Value <- value

            override _.Parse(value) =
                match value with
                | :? DateTime as dt -> DateOnly.FromDateTime dt
                | :? DateOnly as _to -> _to
                | _ -> failwith $"Unexpected value type for DateOnly: {value.GetType()}"
        // End TypeHandlers Module

    module private Helpers =
        let getFieldsStr (fields : string seq) : string =
            String.Join (",", fields)

        let getInsertValueStr (fields : string seq) : string =
            (fields |> Seq.fold (fun acc field -> $"{acc} @{field},") "").Trim().Trim ','

        let getUpdateValueStr (fields : string seq) : string =
            let str     = fields |> Seq.fold (fun acc field -> $"{acc} {field} = @{field},") ""
            let trimmed = str.Trim().Trim ','

            trimmed

        let getConditionStr<'Y> (condition : Condition<'Y>) : string =
            let value =
                match condition.value with
                | Some value -> value.ToString()
                | None -> "NULL"

            $"{condition.column}::VARCHAR {condition.operator} {value}::VARCHAR"

        let getConditionsStr<'Y> (conditions : Condition<'Y> seq) : string =
            conditions |> Seq.fold (fun acc condition -> $"{acc} AND {getConditionStr condition}") "TRUE"

        let getParamConditionStr<'P when 'P : null> (conditions : Condition<'P> seq) : string * obj =
            let columnValue =
                seq {
                    for condition in conditions ->
                        let value =
                            match condition.value with
                            | Some value -> value
                            | None -> null

                        condition.column, value
                } |> Map.ofSeq |> Helpers.DynamicObject.ofMap

            let folder = fun acc condition ->
                let operatorWithRHS : string =
                    match condition.operator.ToLower() = "IN".ToLower() with
                    | true -> $"= ANY(@{condition.column})"
                    | false -> $"{condition.operator} @{condition.column}"

                $"{acc} AND {condition.column}::VARCHAR {operatorWithRHS}"

            let conditionsStr = conditions |> Seq.fold folder "TRUE"

            conditionsStr, columnValue

        let getFieldFromAggregateOperation (operation : AggregateOperation) : string =
            match operation with
                | AggregateOperation.Count param -> $"COUNT({param})"
                | AggregateOperation.Avg param -> $"AVG({param})"
                | AggregateOperation.Sum param -> $"SUM({param})"
                | AggregateOperation.Max param -> $"MAX({param})"
                | AggregateOperation.Min param -> $"MIN({param})"

        let getJoinStr<'Z> (join : Join<'Z>) : string =
            let { _type = _type; table = table; condition = condition } = join
            let conditionStr = getConditionStr condition

            $"{_type} JOIN {table} ON {conditionStr}"
        // End Helpers Module

    let private getConnectionString () =
        let host     = Configs.Database.host
        let port     = Configs.Database.port
        let database = Configs.Database.database
        let username = Configs.Database.username
        let password = Configs.Database.password

        $"Server={host};Port={port};Database={database};User Id={username};Password={password};"

    let private getConnection () : IDbConnection =
        new NpgsqlConnection(getConnectionString())

    let private configurePostgresDatabase () =
        // Test connection
        let connection = getConnection()

        if connection = null then
            raise (DatabaseConnectionError "Error connecting with PostgreSQL DBMS")

        SqlMapper.AddTypeHandler(TypeHandlers.DateOnlyHandler())
        SqlMapper.AddTypeHandler(TypeHandlers.TimeOnlyHandler())
        SqlMapper.AddTypeHandler(TypeHandlers.TimeOnlyOptionHandler())
        PostgreSQL.OptionTypes.register()

    let private useConnection (useCase : IDbConnection -> 'T) : 'T =
        let dsBuilder  = new NpgsqlDataSourceBuilder(getConnectionString())
        let datasource = dsBuilder.Build()
        let connection = datasource.OpenConnection()

        if connection = null then
            raise (DatabaseConnectionError "Error connecting with PostgreSQL DBMS")

        try
            try
                connection |> useCase
            with __ -> reraise()
        finally
            connection.Close()


    let private execute (_param : ExecuteParameter) : int =
        useConnection (
            fun connection ->
                let (sql : string, paramWrapper : obj option) =
                    match _param with
                    | ExecuteParameter.QueryOnly sql -> sql, Some { new Object() with member _.ToString() = "" }
                    | ExecuteParameter.WithParam (sql, param) -> sql, Some param

                connection.Execute (CommandDefinition (sql, paramWrapper.Value))
        )

    let private insert<'T> (table : string) (fields : string seq) (value : 'T) : 'T =
        useConnection (
            fun connection ->
                let fieldsStr     = Helpers.getFieldsStr fields
                let valueStr      = Helpers.getInsertValueStr fields

                let sql  = $"INSERT INTO {table} ({fieldsStr}) VALUES ({valueStr}) RETURNING *"

                connection.QuerySingle<'T> (CommandDefinition (sql, value))
        )

    let private update<'T, 'P> (table : string) (fields : string seq) (value : 'T) (conditions : Condition<'P> seq) : 'T =
        useConnection (
            fun connection ->
                let valueStr      = Helpers.getUpdateValueStr fields
                let conditionsStr = Helpers.getConditionsStr conditions

                let sql  = $"UPDATE {table} SET {valueStr} WHERE {conditionsStr} RETURNING *"

                connection.QuerySingle<'T> (CommandDefinition (sql, value :> obj))
        )

    let private delete<'T, 'P> (table : string) (conditions : Condition<'P> seq) : int =
        useConnection (
            fun connection ->
                let conditionsStr = Helpers.getConditionsStr conditions

                let sql  = $"DELETE FROM {table} WHERE {conditionsStr}"

                connection.Execute (CommandDefinition sql)
        )

    let private prepareSelectParams<'Y, 'Z when 'Y : null and 'Z : null> (table : string) (joins : Join<'Z> seq) (conditions : Condition<'Y> seq) : string * obj =
        let conditionsStr, columnValue = Helpers.getParamConditionStr conditions
        let joinStr = Seq.fold (fun (acc : string) (join : Join<'Z>) -> $"{acc} {Helpers.getJoinStr join}") "" joins

        let sql  = $"SELECT * FROM {table} {joinStr} WHERE {conditionsStr}"

        sql, columnValue

    let private select<'T, 'Y, 'Z when 'Y : null and 'Z : null> (table : string) (joins : Join<'Z> seq) (conditions : Condition<'Y> seq) : 'T list =
        useConnection (
            fun connection ->
                let sql, columnValue = prepareSelectParams table joins conditions
                let objStr = sprintf "%A" columnValue

                try
                    connection.Query<'T> (sql, columnValue) |> List.ofSeq
                with ex -> raise (new Exception $"{sql} __ {ex.Message} __ {objStr}")
        )

    let private selectWithRelation<'T, 'U, 'P,'Y, 'Z when 'Y : null and 'Z : null> (table : string) (joins : Join<'Z> seq) (conditions : Condition<'Y> seq) (splitOn : Func<'T, 'U, 'P>) : 'P list =
        useConnection (
            fun connection ->
                let sql, columnValue = prepareSelectParams table joins conditions

                connection.Query<'T, 'U, 'P> (sql, splitOn, columnValue) |> List.ofSeq
        )

    let private selectSingle<'T, 'Y, 'Z when 'Y : null and 'Z : null> (table : string) (joins : Join<'Z> seq) (conditions : Condition<'Y> seq) : 'T =
        useConnection (
            fun connection ->
                let sql, columnValue = prepareSelectParams table joins conditions

                connection.QuerySingleOrDefault<'T> (sql, columnValue)
        )

    let private selectScalar<'U, 'P when 'P : null> (table : string) (operation : AggregateOperation) (conditions : Condition<'P> seq) : 'U =
        useConnection (
            fun connection ->
                let conditionsStr, columnValue = Helpers.getParamConditionStr conditions
                let field = Helpers.getFieldFromAggregateOperation operation

                let sql  = $"SELECT {field} FROM {table} WHERE {conditionsStr}"

                connection.ExecuteScalar<'U> (sql, columnValue)
        )

    let operations : Operations<'T, 'U, 'P, 'Y, 'Z> =
        {
            insert             = insert
            update             = update
            delete             = delete
            select             = select
            execute            = execute
            selectSingle       = selectSingle
            selectScalar       = selectScalar
            selectWithRelation = selectWithRelation
            configureDatabase  = configurePostgresDatabase
        }

