namespace App.Repositories

open Core.Entities
open Infrastructure.Repositories

/// <summary>
/// Delivery time rule offdays entity repository
/// </summary>
[<AutoOpen>]
module DeliveryTimeRuleNotAvailableWeekday =
    let DeliveryTimeRuleNotAvailableWeekdayRepository : Repository<DeliveryTimeRuleNotAvailableWeekday | null> = {
        Repository.Default with
            table = "delivery_time_rule_not_available_weekdays"
            fillable = [
                "delivery_time_rule_id"
                "weekday_id"
            ]
    }

