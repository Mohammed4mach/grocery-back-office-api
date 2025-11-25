namespace Core.Entities

open System

[<CLIMutable>]
type Order = {
    id: int
    total_cost: float
    order_time: DateTime
    delivery_date: DateOnly option
    delivery_time: TimeOnly option
    is_green_delivery: bool
    user_id: int
    customer_id: int
}

