namespace Configs

open System

module App =
  let name     = Environment.GetEnvironmentVariable("APP_NAME")
  let env      = Environment.GetEnvironmentVariable("APP_ENV")
  let timezone = Environment.GetEnvironmentVariable("APP_TIMEZONE")

module Database =
  let connection = Environment.GetEnvironmentVariable("DB_CONNECTION")
  let host       = Environment.GetEnvironmentVariable("DB_HOST")
  let port       = Environment.GetEnvironmentVariable("DB_PORT")
  let database   = Environment.GetEnvironmentVariable("DB_DATABASE")
  let username   = Environment.GetEnvironmentVariable("DB_USERNAME")
  let password   = Environment.GetEnvironmentVariable("DB_PASSWORD")

