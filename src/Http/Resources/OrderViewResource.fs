namespace Http.Resources

open System
open Core.Entities

type OrderViewResourceData =
    {
        id: int
        total_cost: float
        order_time: DateTime
        delivery_date: Nullable<DateOnly>
        delivery_time: Nullable<TimeOnly>
        is_green_delivery: bool
        user_id: int
        customer_id: int
        user_name: string
        customer_name: string
        notes: string
    }

    static member ofEntity (order : OrderView) : OrderViewResourceData =
        {
            id                = order.id
            total_cost        = order.total_cost
            order_time        = order.order_time
            delivery_date     = order.delivery_date
            delivery_time     = order.delivery_time
            is_green_delivery = order.is_green_delivery
            user_id           = order.user_id
            customer_id       = order.customer_id
            user_name         = order.user_name
            customer_name     = order.customer_name
            notes             = order.notes
        }

type OrderViewResource =
    {
        data : OrderViewResourceData
    }

    static member ofEntity (order : OrderView) : OrderViewResource =
        { data = OrderViewResourceData.ofEntity order }

