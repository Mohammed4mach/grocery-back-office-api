namespace Core.Entities

open System

[<CLIMutable>]
type DeliveryTimeRule = {
    id: int
    name: string
    in_advance_days: int
    same_day_deadline: Nullable<TimeOnly>
}

