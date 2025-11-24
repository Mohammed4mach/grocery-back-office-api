namespace Http.Routes

open Giraffe
open Http.Handlers

module DeliveryTimeRuleNotAvailableWeekdays =
    let routes<'T> =
        subRoute "/delivery-rule-off-days"
            (choose [
                GET  >=> route "" >=> CustomerHandlers.index
            ])

