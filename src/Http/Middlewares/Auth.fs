namespace Http.Middlewares

open Microsoft.AspNetCore.Authentication.Cookies
open Giraffe
open Core.Enums.Http
open Helpers.Response

/// <summary>
/// Module that holds auth specific middlewares
/// </summary>
module Auth =

    let authFailedHandler =
        (negotiate (getErrorBody "Unauthenticated" (int HttpStatus.Unauthorized))) |>
        RequestErrors.unauthorized CookieAuthenticationDefaults.AuthenticationScheme Configs.Auth.realm

    let authenticated<'T> = authFailedHandler |> requiresAuthentication

