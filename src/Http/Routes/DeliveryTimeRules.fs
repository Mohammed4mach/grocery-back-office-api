namespace Http.Routes

open Giraffe
open Http.Handlers

/// <summary>
/// Delivery time rules routes
/// </summary>
module DeliveryTimeRules =
    let routes<'T> =
        choose [
            subRoute "/delivery-rules"
                (choose [
                    GET  >=> route "" >=> DeliveryTimeRuleHandlers.index
                    GET  >=> routef "/%i" DeliveryTimeRuleHandlers.show
                    POST >=> route "" >=> DeliveryTimeRuleHandlers.store
                    PUT >=> routef "/%i" DeliveryTimeRuleHandlers.update
                    DELETE >=> routef "/%i" DeliveryTimeRuleHandlers.delete
                    POST >=> routef "/%i/off-days" DeliveryTimeRuleHandlers.addOffday
                ])
            DELETE >=> routef "/delivery-rule-off-days/%i" DeliveryTimeRuleHandlers.removeOffday
        ]

