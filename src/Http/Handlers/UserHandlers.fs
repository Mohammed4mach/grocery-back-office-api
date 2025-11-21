namespace Http.Handlers

open Microsoft.AspNetCore.Http
open Giraffe
open Infrastructure.Core.Types
open App.Services
open Http.Resources

module UserHandlers =
    let index : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            let filters : Condition seq = []
            let users      = UserService.index filters
            let collection = UserCollection.ofEntity users

            negotiate collection next ctx

