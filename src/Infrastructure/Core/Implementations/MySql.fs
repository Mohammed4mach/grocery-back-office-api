namespace Infrastructure.Core.Implementations

open System
open System.Data
open Npgsql
open Dapper
open Dapper.FSharp
open Infrastructure.Core.Types
open Infrastructure.Core.Exceptions

module MySql =
    let private getConnectionString () =
        let host     = Configs.Database.host
        let port     = Configs.Database.port
        let database = Configs.Database.database
        let username = Configs.Database.username
        let password = Configs.Database.password

        $"Server={host};Port={port};Database={database};User Id={username};Password={password};"

    let mutable private  connectionWrapper : IDbConnection option = Option.None

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
            connection.Open()
            connection |> useCase
        finally
            connection.Close()

    let private execute (_param : ExecuteParameter) : int =
        useConnection (
            fun connection ->
                let (sql : string, paramWrapper : obj option) =
                    match _param with
                    | ExecuteParameter.QueryOnly sql -> (sql, Some { new Object() with member _.ToString() = "" })
                    | ExecuteParameter.WithParam (sql, param) -> (sql, Some param)

                connection.Execute(CommandDefinition (sql, paramWrapper.Value))
        )

    let operations : Operations =
        {
            configureDatabase = configurePostgresDatabase
            execute = execute
        }

