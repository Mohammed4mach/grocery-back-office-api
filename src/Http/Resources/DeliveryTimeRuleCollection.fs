namespace Http.Resources

open Core.Entities

type DeliveryTimeRuleCollection =
    {
        data : DeliveryTimeRuleResourceData seq
    }

    static member ofEntity (rules : DeliveryTimeRule seq) : DeliveryTimeRuleCollection =
        let data : DeliveryTimeRuleResourceData seq =
            seq {
                for rule in rules do
                    yield DeliveryTimeRuleResourceData.ofEntity rule
            }

        { data = data }

