namespace Http.Handlers

open Microsoft.AspNetCore.Http
open Giraffe
open Helpers.Validation
open Core.Entities
open Infrastructure.Core.Types
open App.Services
open Http.Resources
open Http.Requests

module UserHandlers =
    let index : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            let filters : Condition seq = []
            let users      = UserService.index filters
            let collection = UserCollection.ofEntity users

            negotiate collection next ctx

    let show (id : int) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            let user = UserService.show (id.ToString())
            let resource = UserResource.ofEntity user

            negotiate resource next ctx

    let store : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            bindModel<StoreUserRequest> None (
                fun request ->
                    validate request

                    let user : User = {
                        id       = 0
                        fullname = request.fullname
                        username = request.username
                        password = request.password
                    }

                    UserService.store user

                    Successful.CREATED (negotiate "")
            ) next ctx

