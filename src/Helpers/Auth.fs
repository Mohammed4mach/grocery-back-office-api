namespace Helpers

open System
open System.Security.Claims
open Microsoft.AspNetCore.Http
open Core.Exceptions.Authentication

/// <summary>
/// Module that contains helpers that facilitates authentication and authorization
/// </summary>
module Auth =

    /// <summary>
    /// Get a claim of current authenticated user
    /// </summary>
    /// <param name="ctx">The current HTTP context</param>
    /// <param name="_type">Claim type</param>
    /// <returns>The claim or null if the type is not found</returns>
    let private getClaim (ctx : HttpContext) (_type : string) : Claim | null =
        let claim = ctx.User.FindFirst ClaimTypes.NameIdentifier

        if claim = null then
            raise (AuthenticationException "Unauthenticated")

        claim

    /// <summary>
    /// Get current authenticated user ID
    /// </summary>
    /// <param name="ctx">The current HTTP context</param>
    /// <returns>ID of current authenticated user</returns>
    let getUserId (ctx : HttpContext) : int =
        let claim = getClaim ctx ClaimTypes.NameIdentifier

        int claim.Value

    /// <summary>
    /// Indicates whether the current user is a super user
    /// </summary>
    /// <param name="ctx">The current HTTP context</param>
    /// <returns>True if the user is a super user</returns>
    let isSuper (ctx : HttpContext) : bool =
        let claim = getClaim ctx "is_super"
        let charArr = claim.Value |> Seq.toArray
        let span  = new ReadOnlySpan<char>(charArr)
        let mutable super = false;

        try
            Boolean.TryParse(span, &super) |> ignore

            super || claim.Value = "1"
        with ex -> false

