namespace Infrastructure.Repositories

open System

module Order =
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

