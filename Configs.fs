namespace Configs

open DotNetEnv

module App =
    let mutable name     = Env.GetString "APP_NAME"
    let mutable env      = Env.GetString "APP_ENV"
    let mutable timezone = Env.GetString "APP_TIMEZONE"

module Database =
    let mutable connection = Env.GetString "DB_CONNECTION"
    let mutable host       = Env.GetString "DB_HOST"
    let mutable port       = Env.GetInt "DB_PORT"
    let mutable database   = Env.GetString "DB_DATABASE"
    let mutable username   = Env.GetString "DB_USERNAME"
    let mutable password   = Env.GetString "DB_PASSWORD"

module Auth =
    let mutable realm = App.name

module Helpers =
    let refreshConfigs() =
        App.name            <- Env.GetString "APP_NAME"
        App.env             <- Env.GetString "APP_ENV"
        App.timezone        <- Env.GetString "APP_TIMEZONE"
        Database.connection <- Env.GetString "DB_CONNECTION"
        Database.host       <- Env.GetString "DB_HOST"
        Database.port       <- Env.GetInt "DB_PORT"
        Database.database   <- Env.GetString "DB_DATABASE"
        Database.username   <- Env.GetString "DB_USERNAME"
        Database.password   <- Env.GetString "DB_PASSWORD"
        Auth.realm          <- App.name

