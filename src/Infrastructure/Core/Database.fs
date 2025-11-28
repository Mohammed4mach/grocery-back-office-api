namespace Infrastructure.Core

open Infrastructure.Core.Types
open Infrastructure.Core.Exceptions

/// <summary>
/// Module holds the abstract inteface for dealing with the database
/// </summary>
module Database =
    type private Connections =
        | PgSql
        | MySql
        | MsSql

    let private pgsqlOperations = Implementations.PostgreSQL.operations
    // let private mysqlOperations = Implementations.MySql.operations
    // let private mssqlOperations = Implementations.MsSql.operations

    let private pgsqlKey = Connections.PgSql.ToString().ToLower()
    // let private mysqlKey = Connections.MySql.ToString().ToLower()
    // let private mssqlKey = Connections.MsSql.ToString().ToLower()

    /// <summary>
    /// Type that holds abstract database operations interface
    /// </summary>
    /// <typeparam name="'T">Type of the entity</typeparam>
    /// <typeparam name="'U">
    /// Type of the scalar value retuned by an aggregate. Also serve as
    /// type for entity related to `'T`, this used in `selectWithRelation` LOL!
    /// </typeparam>
    /// <typeparam name="'P">
    /// The type of the result of the `selectWithRelation` funciton
    /// </typeparam>
    /// <typeparam name="'Y">Type of the condition's value</typeparam>
    /// <typeparam name="'Z">Type of the condition's value of the join</typeparam>
    let operations<'T, 'U, 'P, 'Y, 'Z when 'Y : null and 'Z : null> : Operations<'T, 'U, 'P, 'Y, 'Z> =
        match Configs.Database.connection with
            | connection when connection = pgsqlKey -> pgsqlOperations
            // | connection when connection = mysqlKey -> mysqlOperations
            // | connection when connection = mssqlKey -> mssqlOperations
            | __ -> raise (DatabaseChoosingError $"Database connection choosed is not valid. Check your configs {__}")

    /// <summary>
    /// Apply the proper configurations to the database connections
    /// </summary>
    let configure () : unit =
        operations.configureDatabase()

