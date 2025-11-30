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
            let filters    : Condition<string> seq = []
            let orders     : OrderView seq         = OrderService.index filters
            let collection : OrderViewCollection   = OrderViewCollection.ofEntity orders

            negotiate collection next ctx

    let show (id : int) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            let order    : OrderView         = OrderService.showView id
            let resource : OrderViewResource = OrderViewResource.ofEntity order

            negotiate resource next ctx

    let getDeliveryTimes (id : int) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            let order : Order = OrderService.show id

            // Get delivery time
            let times    : DeliveryTimes seq      = DeliveryTimeService.getDeliveryTimes order
            let resource : DeliveryTimeCollection = DeliveryTimeCollection.ofEntity times

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
                                    quantity   = item.quantity.Value
                                    product_id = item.product_id.Value
                            }
                    )

                    let resource = OrderResource.ofEntity (OrderService.store order items)

                    negotiate resource
            ) next ctx

    let update (id : int) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            bindModel<UpdateOrderRequest> None (
                fun request ->
                    validate request

                    let deliveryTime : DeliveryTime = {
                        date = DateOnly.Parse request.delivery_date
                        time = TimeOnly.Parse request.delivery_time
                    }

                    let updatedOrder : Order         = deliveryTime |> OrderService.setDeliveryTime id
                    let resource     : OrderResource = OrderResource.ofEntity updatedOrder

                    negotiate resource
            ) next ctx

    let delete (id : int) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            OrderService.delete id

            Successful.NO_CONTENT next ctx

