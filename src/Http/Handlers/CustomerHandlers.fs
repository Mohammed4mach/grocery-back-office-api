namespace Http.Handlers

open Microsoft.AspNetCore.Http
open Giraffe
open Helpers.Validation
open Core.Entities
open Infrastructure.Core.Types
open App.Services
open Http.Resources
open Http.Requests

module CustomerHandlers =
    let index : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            let filters : Condition<string> seq = []
            let customers  = CustomerService.index filters
            let collection = CustomerCollection.ofEntity customers

            negotiate collection next ctx

    let show (id : int) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            let customer = CustomerService.show id
            let resource = CustomerResource.ofEntity customer

            negotiate resource next ctx

    let store : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            bindModel<StoreCustomerRequest> None (
                fun request ->
                    validate request

                    let customer : Customer = {
                        id       = 0
                        fullname = request.fullname
                        address  = request.address
                    }

                    let resource = CustomerResource.ofEntity (CustomerService.store customer)

                    negotiate resource
            ) next ctx

    let update (id : int) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            bindModel<UpdateCustomerRequest> None (
                fun request ->
                    request.id <- id

                    validate request

                    let customer : Customer = {
                        id       = request.id
                        fullname = request.fullname
                        address  = request.address
                    }

                    let resource = CustomerResource.ofEntity (CustomerService.update id customer)

                    negotiate resource
            ) next ctx

    let delete (id : int) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            CustomerService.delete id

            Successful.NO_CONTENT next ctx

