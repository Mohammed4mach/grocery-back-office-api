namespace Http.Middlewares

open Microsoft.AspNetCore.Authentication.Cookies
open Giraffe
open Core.Enums.Http
open Helpers.Response

module Auth =
    let authenticated<'T> = requiresAuthentication (RequestErrors.unauthorized "Cookie" Configs.Auth.realm (negotiate (getErrorBody "Unauthenticated" (int HttpStatus.Unauthorized))))

