namespace Configs

open System

/// <summary>
/// Module that carry app environment variables
/// </summary>
module App =

    /// <summary>
    /// Name of the app
    /// </summary>
    let mutable name = Environment.GetEnvironmentVariable "GROCERY_API_APP_NAME"

    /// <summary>
    /// Envronment type of the app e.g. (local, live, etc...)
    /// </summary>
    let mutable env = Environment.GetEnvironmentVariable "GROCERY_API_APP_ENV"

/// <summary>
/// Module that carry database connection variables
/// </summary>
module Database =

    /// <summary>
    /// DBMS type e.g. (mysql, pgsql, mssql, etc...)
    /// </summary>
    let mutable connection = Environment.GetEnvironmentVariable "GROCERY_API_DB_CONNECTION"

    /// <summary>
    /// Host address of database provider
    /// </summary>
    let mutable host = Environment.GetEnvironmentVariable "GROCERY_API_DB_HOST"

    /// <summary>
    /// Port on which the database is provided
    /// </summary>
    let mutable port = Environment.GetEnvironmentVariable "GROCERY_API_DB_PORT"

    /// <summary>
    /// Database name
    /// </summary>
    let mutable database = Environment.GetEnvironmentVariable "GROCERY_API_DB_DATABASE"

    /// <summary>
    /// Username, for authentication
    /// </summary>
    let mutable username = Environment.GetEnvironmentVariable "GROCERY_API_DB_USERNAME"

    /// <summary>
    /// Password, for authentication
    /// </summary>
    let mutable password = Environment.GetEnvironmentVariable "GROCERY_API_DB_PASSWORD"

/// <summary>
/// Module that holds auth specific configs
/// </summary>
module Auth =

    /// <summary>
    /// Realm of the authentication
    /// </summary>
    let mutable realm = App.name

