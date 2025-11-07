namespace Infrastructure.Repositories

open System

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

