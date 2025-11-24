namespace Http.Handlers

open System.Security.Claims
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Authentication.Cookies
open Giraffe
open App.Services
open Http.Resources
open Http.Requests

module AuthHandlers =
    let login : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                let! request = ctx.BindModelAsync<LoginRequest>()

                Helpers.Validation.validate request

                let user       = AuthService.authenticateUser request.username request.password
                let authScheme = CookieAuthenticationDefaults.AuthenticationScheme
                let claims = [
                    Claim(ClaimTypes.NameIdentifier, user.id.ToString())
                    Claim("is_super", user.is_super.ToString())
                ]

                let claimsIdentity  = new ClaimsIdentity(claims, authScheme)
                let claimsPrincipal = new ClaimsPrincipal(claimsIdentity)

                do! ctx.SignInAsync(authScheme, claimsPrincipal)

                return! negotiate (UserResource.ofEntity user) next ctx
            }

    let logout : HttpHandler = signOut CookieAuthenticationDefaults.AuthenticationScheme >=> Successful.NO_CONTENT

