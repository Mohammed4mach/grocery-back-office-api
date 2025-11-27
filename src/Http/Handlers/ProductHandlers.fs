namespace Http.Handlers

open Microsoft.AspNetCore.Http
open Giraffe
open Helpers.Validation
open Core.Entities
open Infrastructure.Core.Types
open App.Services
open Http.Resources
open Http.Requests

module ProductHandlers =
    let index : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            let filters : Condition<string> seq = []
            let products  = ProductService.index filters
            let collection = ProductCollection.ofEntity products

            negotiate collection next ctx

    let show (id : int) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            let product, storageType = ProductService.show id
            let resource = ProductWithRelationsResource.ofEntity product storageType

            negotiate resource next ctx

    let store : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            bindModel<StoreProductRequest> None (
                fun request ->
                    validate request

                    let product : Product = {
                        id                      = 0
                        name                    = request.name
                        price                   = request.price
                        description             = request.description
                        product_storage_type_id = request.product_storage_type_id.Value
                    }

                    let resource = ProductResource.ofEntity (ProductService.store product)

                    negotiate resource
            ) next ctx

    let update (id : int) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            bindModel<UpdateProductRequest> None (
                fun request ->
                    request.id <- id

                    validate request

                    let product : Product = {
                        id                      = request.id
                        name                    = request.name
                        price                   = request.price
                        description             = request.description
                        product_storage_type_id = request.product_storage_type_id.Value
                    }

                    let resource = ProductResource.ofEntity (ProductService.update id product)

                    negotiate resource
            ) next ctx

    let delete (id : int) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            ProductService.delete id

            Successful.NO_CONTENT next ctx

