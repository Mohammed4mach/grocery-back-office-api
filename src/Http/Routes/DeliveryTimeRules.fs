namespace Http.Routes

open Giraffe
open Http.Handlers

module DeliveryTimeRules =
    let routes<'T> =
        subRoute "/delivery-rules"
            (choose [
                GET  >=> route "" >=> DeliveryTimeRuleHandlers.index
                GET  >=> routef "/%i" DeliveryTimeRuleHandlers.show
                POST >=> route "" >=> DeliveryTimeRuleHandlers.store
                PUT >=> routef "/%i" DeliveryTimeRuleHandlers.update
                DELETE >=> routef "/%i" DeliveryTimeRuleHandlers.delete
            ])

