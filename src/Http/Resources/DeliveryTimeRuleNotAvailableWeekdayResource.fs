namespace Http.Resources

open Core.Entities

type DeliveryTimeRuleNotAvailableWeekdayResourceData =
    {
        id: int
        delivery_time_rule_id: int
        weekday_id: int
    }

type DeliveryTimeRuleNotAvailableWeekdayResource =
    {
        data : DeliveryTimeRuleNotAvailableWeekdayResourceData
    }

    static member ofEntity (rule : DeliveryTimeRuleNotAvailableWeekday) : DeliveryTimeRuleNotAvailableWeekdayResource =
        let data : DeliveryTimeRuleNotAvailableWeekdayResourceData = {
            id                    = rule.id
            delivery_time_rule_id = rule.delivery_time_rule_id
            weekday_id            = rule.weekday_id
        }

        let resource : DeliveryTimeRuleNotAvailableWeekdayResource = { data = data }

        resource

