namespace Helpers

open System
open System.Security.Claims
open Microsoft.AspNetCore.Http
open Core.Exceptions.Authentication

module Auth =
    let private getClaim (ctx : HttpContext) (_type : string) : Claim | null =
        let claim = ctx.User.FindFirst ClaimTypes.NameIdentifier

        if claim = null then
            raise (AuthenticationException "Unauthenticated")

        claim

    let getUserId (ctx : HttpContext) : int =
        let claim = getClaim ctx ClaimTypes.NameIdentifier

        int claim.Value

    let isSuper (ctx : HttpContext) : bool =
        let claim = getClaim ctx "is_super"
        let charArr = claim.Value |> Seq.toArray
        let span  = new ReadOnlySpan<char>(charArr)
        let mutable super = false;

        try
            Boolean.TryParse(span, &super) |> ignore

            super || claim.Value = "1"
        with ex -> false

