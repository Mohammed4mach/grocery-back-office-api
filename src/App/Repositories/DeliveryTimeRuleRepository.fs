namespace App.Repositories

open Core.Entities
open Infrastructure.Repositories

/// <summary>
/// Delivery time rule entity repository
/// </summary>
[<AutoOpen>]
module DeliveryTimeRule =
    let DeliveryTimeRuleRepository : Repository<DeliveryTimeRule | null> = {
        Repository.Default with
            table = "delivery_time_rules"
            fillable = [
                "name"
                "in_advance_days"
                "same_day_deadline"
            ]
    }

