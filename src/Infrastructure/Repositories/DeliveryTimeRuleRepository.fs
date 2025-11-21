namespace Infrastructure.Repositories

open System

module DeliveryTimeRule =
    type DeliveryTimeRule = {
        id: int
        name: string
        inAdvanceDays: int
        sameDayDeadline: DateTime
    }

