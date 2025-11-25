namespace Http.Resources

open System
open Core.Entities

type DeliveryTimeRuleResourceData =
    {
        id: int
        name: string
        in_advance_days: int
        same_day_deadline: Nullable<TimeOnly>
    }

    static member ofEntity (rule : DeliveryTimeRule) : DeliveryTimeRuleResourceData =
        {
            id                = rule.id
            name              = rule.name
            in_advance_days   = rule.in_advance_days
            same_day_deadline = rule.same_day_deadline
        }

type DeliveryTimeRuleResource =
    {
        data : DeliveryTimeRuleResourceData
    }

    static member ofEntity (rule : DeliveryTimeRule) : DeliveryTimeRuleResource =
        { data = DeliveryTimeRuleResourceData.ofEntity rule }

