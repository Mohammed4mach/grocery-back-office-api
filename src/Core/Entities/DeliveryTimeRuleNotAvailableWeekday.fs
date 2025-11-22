namespace Core.Entities

[<CLIMutable>]
type DeliveryTimeRuleNotAvailableWeekday = {
    id: int
    delivery_time_rule_id: int
    weekday_id: int
}

