namespace Core

open System
open Microsoft.Extensions.Logging
open Giraffe
open Helpers.Response
open Core.Enums.Http
open Core.Exceptions
open Core.Exceptions.Validation
open Core.Exceptions.Authentication
open Core.Exceptions.Authorization

module ErrorHandlers =
    let mainHandler (ex : Exception) (logger : ILogger) =
        clearResponse
        >=> match ex with
            | BadRequestError message
            | MaxLengthExceededError message
            | MaxValueExceededError message
            | MinLengthError message
            | MinValueError message
            | NotUniqueValidationError message
            | UnmatchedFormatError message
            | RequiredFieldError message -> RequestErrors.badRequest (negotiate (getErrorBody message (int HttpStatus.Bad_Request)))
            | ConflictError message -> RequestErrors.conflict (negotiate (getErrorBody message (int HttpStatus.Bad_Request)))
            | AuthenticationException message -> RequestErrors.unauthorized "Cookie" Configs.Auth.realm (negotiate (getErrorBody message (int HttpStatus.Unauthorized)))
            | AuthorizationException message -> RequestErrors.forbidden (negotiate (getErrorBody message (int HttpStatus.Forbidden)))
            | EntityNotFoundError message -> RequestErrors.notFound (negotiate (getErrorBody message (int HttpStatus.Not_Found)))
            |_ ->
                logger.LogError(ex, "An unhandled exception has occurred while executing the request.")
                setStatusCode 500
                >=> match Configs.App.env with
                        | env when env = "live" -> negotiate {| message = "Unhandled internal server error" |}
                        | _ -> negotiate {| ``type`` = ex.GetType(); message = ex.Message; trace = ex.StackTrace; |}

