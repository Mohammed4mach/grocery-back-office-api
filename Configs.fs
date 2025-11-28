namespace Configs

open DotNetEnv

/// <summary>
/// Module that carry app environment variables
/// </summary>
module App =

    /// <summary>
    /// Name of the app
    /// </summary>
    let mutable name = Env.GetString "APP_NAME"

    /// <summary>
    /// Envronment type of the app e.g. (local, live, etc...)
    /// </summary>
    let mutable env = Env.GetString "APP_ENV"

/// <summary>
/// Module that carry database connection variables
/// </summary>
module Database =

    /// <summary>
    /// DBMS type e.g. (mysql, pgsql, mssql, etc...)
    /// </summary>
    let mutable connection = Env.GetString "DB_CONNECTION"

    /// <summary>
    /// Host address of database provider
    /// </summary>
    let mutable host = Env.GetString "DB_HOST"

    /// <summary>
    /// Port on which the database is provided
    /// </summary>
    let mutable port = Env.GetInt "DB_PORT"

    /// <summary>
    /// Database name
    /// </summary>
    let mutable database = Env.GetString "DB_DATABASE"

    /// <summary>
    /// Username, for authentication
    /// </summary>
    let mutable username = Env.GetString "DB_USERNAME"

    /// <summary>
    /// Password, for authentication
    /// </summary>
    let mutable password = Env.GetString "DB_PASSWORD"

/// <summary>
/// Module that holds auth specific configs
/// </summary>
module Auth =

    /// <summary>
    /// Realm of the authentication
    /// </summary>
    let mutable realm = App.name

/// <summary>
/// Module contains helpers for configs
/// </summary>
module Helpers =

    /// <summary>
    /// Reload the configs from .env file
    /// </summary>
    let refreshConfigs() : unit =
        App.name            <- Env.GetString "APP_NAME"
        App.env             <- Env.GetString "APP_ENV"
        Database.connection <- Env.GetString "DB_CONNECTION"
        Database.host       <- Env.GetString "DB_HOST"
        Database.port       <- Env.GetInt "DB_PORT"
        Database.database   <- Env.GetString "DB_DATABASE"
        Database.username   <- Env.GetString "DB_USERNAME"
        Database.password   <- Env.GetString "DB_PASSWORD"
        Auth.realm          <- App.name

