namespace Http.Resources

open System
open Core.Entities

type OrderResourceData =
    {
        id: int
        total_cost: float
        order_time: DateTime
        delivery_date: DateTime
        delivery_time: DateTime
        is_green_delivery: bool
        user_id: int
        customer_id: int
    }

type OrderResource =
    {
        data : OrderResourceData
    }

    static member ofEntity (order : Order) : OrderResource =
        let data : OrderResourceData = {
            id                = order.id
            total_cost        = order.total_cost
            order_time        = order.order_time
            delivery_date     = order.delivery_date
            delivery_time     = order.delivery_time
            is_green_delivery = order.is_green_delivery
            user_id           = order.user_id
            customer_id       = order.customer_id
        }

        let resource : OrderResource = { data = data }

        resource

