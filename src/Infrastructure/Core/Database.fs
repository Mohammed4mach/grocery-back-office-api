namespace Infrastructure.Core

open Infrastructure.Core.Types
open Infrastructure.Core.Exceptions

module Database =
    type private Connections =
        | PgSql
        | MySql
        | MsSql

    let private pgsqlOperations = Implementations.PostgreSQL.operations
    let private mysqlOperations = Implementations.MySql.operations
    let private mssqlOperations = Implementations.MsSql.operations

    let private pgsqlKey = Connections.PgSql.ToString().ToLower()
    let private mysqlKey = Connections.MySql.ToString().ToLower()
    let private mssqlKey = Connections.MsSql.ToString().ToLower()

    let operations<'T> : Operations<'T> =
        match Configs.Database.connection with
            | connection when connection = pgsqlKey -> pgsqlOperations
            | connection when connection = mysqlKey -> mysqlOperations
            | connection when connection = mssqlKey -> mssqlOperations
            | __ -> raise (DatabaseChoosingError $"Database connection choosed is not valid. Check your configs {__}")

    let configure () : unit =
        operations.configureDatabase()

