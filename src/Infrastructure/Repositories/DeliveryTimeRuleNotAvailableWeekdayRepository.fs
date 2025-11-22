namespace Infrastructure.Repositories

open Core.Entities

[<AutoOpen>]
module DeliveryTimeRuleNotAvailableWeekday =
    let DeliveryTimeRuleNotAvailableWeekdayRepository : Repository<DeliveryTimeRuleNotAvailableWeekday> = {
        Repository.Default with
            table = "delivery_time_rule_not_available_weekdays"
            fillable = [
                "id"
                "delivery_time_rule_id"
                "weekday_id"
            ]
    }

