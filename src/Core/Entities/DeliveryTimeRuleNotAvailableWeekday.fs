namespace Core.Entities

/// <summary>
/// Entity that make the bond between delivery time rule and weekdays
/// determining the offdays of a rule
/// </summary>
[<CLIMutable>]
type DeliveryTimeRuleNotAvailableWeekday = {
    id: int
    delivery_time_rule_id: int
    weekday_id: int
}

