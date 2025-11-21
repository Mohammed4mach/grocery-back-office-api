namespace Core.Entities

open System

[<CLIMutable>]
type DeliveryTimeRule = {
    id: int
    name: string
    inAdvanceDays: int
    sameDayDeadline: DateTime
}

