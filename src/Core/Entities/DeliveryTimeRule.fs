namespace Core.Entities

open System

type DeliveryTimeRule = {
    id: int
    name: string
    inAdvanceDays: int
    sameDayDeadline: DateTime
}

