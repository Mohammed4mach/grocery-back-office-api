namespace Core.Entities

open System

[<CLIMutable>]
type Order = {
    id: int
    totalCost: float
    orderTime: DateTime
    deliveryDate: DateTime
    deliveryTime: DateTime
    isGreenDelivery: bool
    userId: int
    customerId: int
}

