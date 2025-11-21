namespace Core.Entities

[<CLIMutable>]
type DeliveryTimeRuleNotAvailableWeekday = {
    id: int
    deliveryTimeRuleId: int
    weekdayId: int
}

