namespace Core.Entities

open System

/// <summary>
/// Entity that model orders resource
/// </summary>
[<CLIMutable>]
type OrderView =
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
    }

