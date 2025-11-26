namespace Http.Handlers

open System
open Microsoft.AspNetCore.Http
open Giraffe
open Helpers.Validation
open Core.Entities
open Infrastructure.Core.Types
open App.Services
open Http.Resources
open Http.Requests

module OrderHandlers =
    let index : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            let filters : Condition seq = []
            let orders  = OrderService.index filters
            let collection = OrderCollection.ofEntity orders

            negotiate collection next ctx

    let show (id : int) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            let order = OrderService.show id
            let resource = OrderResource.ofEntity order

            negotiate resource next ctx

    let store : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            bindModel<StoreOrderRequest> None (
                fun request ->
                    let id = Helpers.Auth.getUserId ctx

                    request.user_id <- id

                    validate request

                    let order : Order = {
                        Order.Default with
                            order_time  = DateTime.Now
                            user_id     = request.user_id
                            customer_id = request.customer_id
                    }

                    let requestItems : OrderItemData seq =
                        match request.items with
                        | null -> Seq.empty
                        | items -> items

                    let items : OrderItem seq = requestItems |> Seq.map<OrderItemData, OrderItem> (
                        fun (item : OrderItemData) ->
                            {
                                OrderItem.Default with
                                    quantity      = item.quantity.Value
                                    product_id    = item.product_id.Value
                            }
                    )

                    let resource = OrderResource.ofEntity (OrderService.store order items)

                    negotiate resource
            ) next ctx

    let update (id : int) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            bindModel<UpdateOrderRequest> None (
                fun request ->
                    request.id <- id

                    validate request

                    let order : Order = {
                        id                = request.id
                        total_cost        = 0.00
                        order_time        = DateTime.Now
                        delivery_date     = Nullable()
                        delivery_time     = Nullable()
                        is_green_delivery = false
                        user_id           = 0
                        customer_id       = 0
                    }

                    let resource = OrderResource.ofEntity (OrderService.update id order)

                    negotiate resource
            ) next ctx

    let delete (id : int) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            OrderService.delete id

            Successful.NO_CONTENT next ctx

