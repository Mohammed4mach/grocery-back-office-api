namespace Core.Entities

open System

[<CLIMutable>]
type Order =
    {
        id: int
        total_cost: float
        order_time: DateTime
        delivery_date: Nullable<DateOnly>
        delivery_time: Nullable<TimeOnly>
        is_green_delivery: bool
        user_id: int
        customer_id: int
    }

    static member Default : Order =
        {
            id                = 0
            total_cost        = 0.00
            order_time        = DateTime.Now
            delivery_date     = Nullable()
            delivery_time     = Nullable()
            is_green_delivery = false
            user_id           = 0
            customer_id       = 0
        }

