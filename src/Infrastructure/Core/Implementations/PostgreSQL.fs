namespace Infrastructure.Core.Implementations

open System
open System.Data
open Npgsql
open Dapper
open Dapper.FSharp
open Infrastructure.Core.Types
open Infrastructure.Core.Exceptions

module PostgreSQL =
    let mutable private  connectionWrapper : IDbConnection option = Option.None

    let private getConnectionString () =
        let host     = Configs.Database.host
        let port     = Configs.Database.port
        let database = Configs.Database.database
        let username = Configs.Database.username
        let password = Configs.Database.password

        $"Server={host};Port={port};Database={database};User Id={username};Password={password};"

    let private getConnection () : IDbConnection option =
        match connectionWrapper with
            | Some conn -> Some conn
            | Option.None ->
                let connStr = getConnectionString()
                Some (new NpgsqlConnection(connStr))

    let private configurePostgresDatabase () =
        connectionWrapper <- getConnection()

        if Option.isNone connectionWrapper then
            raise (DatabaseConnectionError ("Error connecting with PostgreSQL DBMS"))

        PostgreSQL.OptionTypes.register()

    let private useConnection (useCase : IDbConnection -> 'T) : 'T =
        let connection =
            match connectionWrapper with
                | Some conn -> conn
                | Option.None ->
                    raise (DatabaseConnectionError ("PostgreSQL connection is not configured"))

        try
            try
                connection.Open()
                connection |> useCase
            with __ -> reraise()
        finally
            connection.Close()

    let private getFieldsStr (fields : string seq) =
        String.Join (",", fields)

    let private getInsertValueStr (fields : string seq) : string =
        (fields |> Seq.fold (fun (acc) (field) -> $"{acc} @{field},") "").Trim ','

    let private getUpdateValueStr (fields : string seq) : string =
        (fields |> Seq.fold (fun (acc) (field) -> $"{acc} {field} = @{field}, ") "").Trim ','

    let private getConditionValue (valueWrapper : string option) =
        match valueWrapper with
        | Some value -> value
        | None -> "NULL"

    let private getConditionStr (condition : Condition) =
        $"{condition.column}::VARCHAR {condition.operator} {getConditionValue condition.value}"

    let private getConditionsStr (conditions : Condition seq) : string =
        conditions |> Seq.fold (fun (acc) (condition) -> $"{acc} AND {getConditionStr condition}") "TRUE"

    let private getParamConditionStr (conditions : Condition seq) : string * obj =
        let columnValue = seq { for condition in conditions -> (condition.column, getConditionValue condition.value) } |> Map.ofSeq |> Helpers.DynamicObject.ofMap

        let conditionsStr = conditions |> Seq.fold (fun (acc) (condition) -> $"{acc} AND {condition.column}::VARCHAR {condition.operator} @{condition.column}") "TRUE"

        (conditionsStr, columnValue)

    let private execute (_param : ExecuteParameter) : int =
        useConnection (
            fun connection ->
                let (sql : string, paramWrapper : obj option) =
                    match _param with
                    | ExecuteParameter.QueryOnly sql -> (sql, Some { new Object() with member _.ToString() = "" })
                    | ExecuteParameter.WithParam (sql, param) -> (sql, Some param)

                connection.Execute (CommandDefinition (sql, paramWrapper.Value))
        )

    let private insert<'T> (table : string) (fields : string seq) (value : 'T) : int =
        useConnection (
            fun connection ->
                let fieldsStr     = getFieldsStr fields
                let valueStr      = getInsertValueStr fields

                let sql  = $"INSERT INTO {table} ({fieldsStr}) VALUES ({valueStr})"

                connection.Execute (CommandDefinition (sql, value))
        )

    let private update<'T> (table : string) (fields : string seq) (value : 'T) (conditions : Condition seq) : int =
        useConnection (
            fun connection ->
                let valueStr      = getUpdateValueStr fields
                let conditionsStr = getConditionsStr conditions

                let sql  = $"UPDATE {table} SET {valueStr} WHERE {conditionsStr}"

                connection.Execute (CommandDefinition (sql, value))
        )

    let private delete<'T> (table : string) (conditions : Condition seq) : int =
        useConnection (
            fun connection ->
                let conditionsStr = getConditionsStr conditions

                let sql  = $"DELETE FROM {table} WHERE {conditionsStr}"

                connection.Execute (CommandDefinition sql)
        )

    let private select<'T> (table : string) (conditions : Condition seq) : 'T list =
        useConnection (
            fun connection ->
                let (conditionsStr, columnValue) = getParamConditionStr conditions

                let sql  = $"SELECT * FROM {table} WHERE {conditionsStr}"

                connection.Query<'T> (sql, columnValue) |> List.ofSeq
        )

    let private selectSingle<'T> (table : string) (conditions : Condition seq) : 'T =
        useConnection (
            fun connection ->
                let (conditionsStr, columnValue) = getParamConditionStr conditions

                let sql  = $"SELECT * FROM {table} WHERE {conditionsStr}"

                connection.QuerySingle<'T> (sql, columnValue)
        )

    let operations : Operations<'T> =
        {
            insert            = insert
            update            = update
            delete            = delete
            select            = select
            execute           = execute
            selectSingle      = selectSingle
            configureDatabase = configurePostgresDatabase
        }

