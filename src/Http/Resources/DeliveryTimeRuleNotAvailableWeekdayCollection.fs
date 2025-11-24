namespace Http.Resources

open Core.Entities

type DeliveryTimeRuleNotAvailableWeekdayCollection =
    {
        data : DeliveryTimeRuleNotAvailableWeekdayResourceData seq
    }

    static member ofEntity (rules : DeliveryTimeRuleNotAvailableWeekday seq) : DeliveryTimeRuleNotAvailableWeekdayCollection =
        let data : DeliveryTimeRuleNotAvailableWeekdayResourceData seq =
            seq {
                for rule in rules do
                    yield DeliveryTimeRuleNotAvailableWeekdayResourceData.ofEntity rule
            }

        { data = data }

