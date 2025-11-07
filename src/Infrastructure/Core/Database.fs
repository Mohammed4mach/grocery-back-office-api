namespace Infrastructure.Core

open Infrastructure.Core.Types
open Infrastructure.Core.Exceptions

module Database =
    type private Connections =
        | PgSql
        | MySql
        | MsSql

    let pgsqlOperations = Implementations.Postgres.operations
    let mysqlOperations = Implementations.MySql.operations
    let mssqlOperations = Implementations.MsSql.operations

    let pgsqlKey = Connections.PgSql.ToString().ToLower()
    let mysqlKey = Connections.MySql.ToString().ToLower()
    let mssqlKey = Connections.MsSql.ToString().ToLower()

    let mutable operations : Operations = pgsqlOperations

    let private _configure () : unit =
        operations <-
            match Configs.Database.connection with
                | connection when connection = pgsqlKey -> pgsqlOperations
                | connection when connection = mysqlKey -> mysqlOperations
                | connection when connection = mssqlKey -> mssqlOperations
                | __ -> raise (DatabaseChoosingError ($"Database connection choice is not valid. Check your configs {__}"))

    let configure () : unit =
        _configure()
        operations.configureDatabase()

