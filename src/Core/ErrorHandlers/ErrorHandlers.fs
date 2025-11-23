namespace Core

open System
open Microsoft.Extensions.Logging
open Giraffe
open Helpers.Response
open Core.Exceptions
open Core.Exceptions.Validation
open Core.Exceptions.Authentication

module ErrorHandlers =
    let mainHandler (ex : Exception) (logger : ILogger) =
        clearResponse
        >=> match ex with
            | EntityNotFoundError message
            | BadRequestError message
            | MaxLengthExceededError message
            | MaxValueExceededError message
            | MinLengthError message
            | MinValueError message
            | NotUniqueValidationError message
            | RequiredFieldError message -> RequestErrors.badRequest (negotiate (getErrorBody (message) 400))
            | AuthenticationException message -> RequestErrors.unauthorized "Cookie" Configs.Auth.realm (negotiate (getErrorBody message 401))
            |_ ->
                logger.LogError(ex, "An unhandled exception has occurred while executing the request.")
                setStatusCode 500 >=> negotiate {| ``type`` = ex.GetType(); message = ex.Message; trace = ex.StackTrace; |}

