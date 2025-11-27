namespace Http.Resources

open System
open Core.Entities

type DeliveryTimeRuleResourceWithRelaitonsData =
    {
        id: int
        name: string
        in_advance_days: int
        same_day_deadline: Nullable<TimeOnly>
        offdays: Weekday seq
    }

    static member ofEntity (rule : DeliveryTimeRule) (offdays : Weekday seq) : DeliveryTimeRuleResourceWithRelaitonsData =
        {
            id                = rule.id
            name              = rule.name
            in_advance_days   = rule.in_advance_days
            same_day_deadline = rule.same_day_deadline
            offdays           = offdays
        }

type DeliveryTimeRuleWithRelationsResource =
    {
        data : DeliveryTimeRuleResourceWithRelaitonsData
    }

    static member ofEntity (rule : DeliveryTimeRule) (offdays : Weekday seq) : DeliveryTimeRuleWithRelationsResource =
        { data = DeliveryTimeRuleResourceWithRelaitonsData.ofEntity rule offdays }

