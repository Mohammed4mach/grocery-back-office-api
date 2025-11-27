namespace Http.Handlers

open Microsoft.AspNetCore.Http
open Giraffe
open Helpers.Validation
open Core.Entities
open Infrastructure.Core.Types
open App.Services
open Http.Resources
open Http.Requests

module ProductStorageTypeHandlers =
    let index : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            let filters : Condition<string> seq = []
            let productTypes = ProductStorageTypeService.index filters
            let collection   = ProductStorageTypeCollection.ofEntity productTypes

            negotiate collection next ctx

    let show (id : int) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            let productType = ProductStorageTypeService.show id
            let resource    = ProductStorageTypeResource.ofEntity productType

            negotiate resource next ctx

    let store : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            bindModel<StoreProductStorageTypeRequest> None (
                fun request ->
                    validate request

                    let productType : ProductStorageType = {
                        id                    = 0
                        name                  = request.name
                        delivery_time_rule_id = request.delivery_time_rule_id
                    }

                    let resource = ProductStorageTypeResource.ofEntity (ProductStorageTypeService.store productType)

                    negotiate resource
            ) next ctx

    let update (id : int) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            bindModel<UpdateProductStorageTypeRequest> None (
                fun request ->
                    request.id <- id

                    validate request

                    let productType : ProductStorageType = {
                        id                    = request.id
                        name                  = request.name
                        delivery_time_rule_id = request.delivery_time_rule_id
                    }

                    let resource = ProductStorageTypeResource.ofEntity (ProductStorageTypeService.update id productType)

                    negotiate resource
            ) next ctx

    let delete (id : int) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            ProductStorageTypeService.delete id

            Successful.NO_CONTENT next ctx

