namespace Infrastructure.Core.Implementations

open System
open System.Data
open Npgsql
open Dapper
open Dapper.FSharp
open Infrastructure.Core.Types
open Infrastructure.Core.Exceptions

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



module private Helpers =
    let getFieldsStr (fields : string seq) : string =
        String.Join (",", fields)

    let getInsertValueStr (fields : string seq) : string =
        (fields |> Seq.fold (fun acc field -> $"{acc} @{field},") "").Trim().Trim ','

    let getUpdateValueStr (fields : string seq) : string =
        let str     = fields |> Seq.fold (fun acc field -> $"{acc} {field} = @{field},") ""
        let trimmed = str.Trim().Trim ','

        trimmed

    let getConditionValue (valueWrapper : string option) : string =
        match valueWrapper with
        | Some value -> value
        | None -> "NULL"

    let getConditionStr (condition : Condition) : string =
        $"{condition.column}::VARCHAR {condition.operator} {getConditionValue condition.value}::VARCHAR"

    let getConditionsStr (conditions : Condition seq) : string =
        conditions |> Seq.fold (fun acc condition -> $"{acc} AND {getConditionStr condition}") "TRUE"

    let getParamConditionStr (conditions : Condition seq) : string * obj =
        let columnValue = seq { for condition in conditions -> condition.column, getConditionValue condition.value } |> Map.ofSeq |> Helpers.DynamicObject.ofMap

        let conditionsStr = conditions |> Seq.fold (fun acc condition -> $"{acc} AND {condition.column}::VARCHAR {condition.operator} @{condition.column}") "TRUE"

        conditionsStr, columnValue

    let getFieldFromAggregateOperation (operation : AggregateOperation) : string =
        match operation with
            | AggregateOperation.Count param -> $"COUNT({param})"
            | AggregateOperation.Avg param -> $"AVG({param})"
            | AggregateOperation.Sum param -> $"SUM({param})"
            | AggregateOperation.Max param -> $"MAX({param})"
            | AggregateOperation.Min param -> $"MIN({param})"

    let getJoinStr (join : Join) : string =
        let { _type = _type; table = table; condition = condition } = join
        let conditionStr = getConditionStr condition

        $"{_type} JOIN {table} ON {conditionStr}"

module PostgreSQL =
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

    let private update<'T> (table : string) (fields : string seq) (value : 'T) (conditions : Condition seq) : 'T =
        useConnection (
            fun connection ->
                let valueStr      = Helpers.getUpdateValueStr fields
                let conditionsStr = Helpers.getConditionsStr conditions

                let sql  = $"UPDATE {table} SET {valueStr} WHERE {conditionsStr} RETURNING *"

                connection.QuerySingle<'T> (CommandDefinition (sql, value :> obj))
        )

    let private delete<'T> (table : string) (conditions : Condition seq) : int =
        useConnection (
            fun connection ->
                let conditionsStr = Helpers.getConditionsStr conditions

                let sql  = $"DELETE FROM {table} WHERE {conditionsStr}"

                connection.Execute (CommandDefinition sql)
        )

    let private prepareSelectParams (table : string) (joins : Join seq) (conditions : Condition seq) : string * obj =
        let conditionsStr, columnValue = Helpers.getParamConditionStr conditions
        let joinStr = Seq.fold (fun (acc : string) (join : Join) -> $"{acc} {Helpers.getJoinStr join}") "" joins

        let sql  = $"SELECT * FROM {table} {joinStr} WHERE {conditionsStr}"

        sql, columnValue

    let private select<'T> (table : string) (joins : Join seq) (conditions : Condition seq) : 'T list =
        useConnection (
            fun connection ->
                let sql, columnValue = prepareSelectParams table joins conditions

                // connection.QueryMultipleAsync
                connection.Query<'T> (sql, columnValue) |> List.ofSeq
        )

    let private selectSingle<'T> (table : string) (joins : Join seq) (conditions : Condition seq) : 'T =
        useConnection (
            fun connection ->
                let sql, columnValue = prepareSelectParams table joins conditions

                connection.QuerySingleOrDefault<'T> (sql, columnValue)
        )

    let private selectScalar<'U> (table : string) (operation : AggregateOperation) (conditions : Condition seq) : 'U =
        useConnection (
            fun connection ->
                let conditionsStr, columnValue = Helpers.getParamConditionStr conditions
                let field = Helpers.getFieldFromAggregateOperation operation

                let sql  = $"SELECT {field} FROM {table} WHERE {conditionsStr}"

                connection.ExecuteScalar<'U> (sql, columnValue)
        )

    let operations : Operations<'T, 'U> =
        {
            insert            = insert
            update            = update
            delete            = delete
            select            = select
            execute           = execute
            selectSingle      = selectSingle
            selectScalar      = selectScalar
            configureDatabase = configurePostgresDatabase
        }

