namespace Http.Handlers

open Microsoft.AspNetCore.Http
open Giraffe
open Helpers.Validation
open Core.Entities
open Core.Exceptions.Authorization
open Core.Enums.Http
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

    let show : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            let id       = Helpers.Auth.getUserId ctx
            let user     = UserService.show id
            let resource = UserResource.ofEntity user

            negotiate resource next ctx

    let store : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            bindModel<StoreUserRequest> None (
                fun request ->
                    let isSuper = Helpers.Auth.isSuper ctx

                    if not isSuper then
                        raise (AuthorizationException "Unauthorized")

                    validate request

                    let user : User = {
                        id       = 0
                        fullname = request.fullname
                        username = request.username
                        password = request.password
                        is_super = request.is_super
                    }

                    UserService.store user

                    Successful.CREATED (negotiate "")
            ) next ctx

    let update : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            bindModel<UpdateUserRequest> None (
                fun request ->
                    let id      = Helpers.Auth.getUserId ctx
                    let isSuper = Helpers.Auth.isSuper ctx

                    request.id <- id

                    validate request

                    let user : User = {
                        id       = request.id
                        fullname = request.fullname
                        username = request.username
                        password = request.password
                        is_super = isSuper
                    }
                    let resource : UserResource = UserResource.ofEntity user

                    UserService.update id user

                    negotiate resource
            ) next ctx

    let updateCredentials : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            bindModel<UpdatePasswordRequest> None (
                fun request ->
                    let id = Helpers.Auth.getUserId ctx

                    validate request

                    UserService.updatePassword id request.password request.new_password

                    setStatusCode (int HttpStatus.OK)
            ) next ctx

    let delete (id : int) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            let isSuper = Helpers.Auth.isSuper ctx

            if not isSuper then
                raise (AuthorizationException "Unauthorized")

            UserService.delete id

            Successful.NO_CONTENT next ctx

