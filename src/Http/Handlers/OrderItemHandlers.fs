namespace Http.Handlers

open System
open Microsoft.AspNetCore.Http
open Giraffe
open Helpers.Validation
open Core.Entities
open Infrastructure.Core.Types
open App.Services
open App.Repositories
open App.Interfaces
open Http.Resources
open Http.Requests

module OrderItemHandlers =
    let index (orderId : int) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            let filters    : Condition<string> seq   = []
            let items      : OrderItemView seq       = OrderItemService.indexView orderId filters
            let collection : OrderItemViewCollection = OrderItemViewCollection.ofEntity items

            negotiate collection next ctx

    let show (id : int) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            let item, product = OrderItemService.show id
            let resource      = OrderItemWithRelationsResource.ofEntity item product

            negotiate resource next ctx

    let store (orderId : int) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            bindModel<StoreOrderItemRequest> None (
                fun request ->
                    request.order_id <- orderId

                    validate request

                    let item : OrderItem =
                        {
                            OrderItem.Default with
                                quantity   = request.quantity
                                product_id = request.product_id
                        }

                    let item : OrderItem             = OrderItemService.store orderId item false
                    let resource : OrderItemResource = OrderItemResource.ofEntity item

                    OrderService.updateOrderTotalCost orderId |> ignore

                    negotiate resource
            ) next ctx

    let update (id : int) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            bindModel<UpdateOrderItemRequest> None (
                fun request ->
                    request.id <- id

                    validate request

                    let item : OrderItem = {
                        OrderItem.Default with
                            quantity = request.quantity
                    }

                    let item     = OrderItemService.updateQuantity id item
                    let resource = OrderItemResource.ofEntity item

                    OrderService.updateOrderTotalCost item.order_id |> ignore

                    negotiate resource
            ) next ctx

    let delete (id : int) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            let itemRepo         = OrderItemRepository :> IRepository<OrderItem | null>
            let item : OrderItem = itemRepo.find (id.ToString()) []

            OrderItemService.delete item.id

            OrderService.updateOrderTotalCost item.order_id |> ignore

            Successful.NO_CONTENT next ctx

