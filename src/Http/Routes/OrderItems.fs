namespace Http.Routes

open Giraffe
open Http.Handlers

/// <summary>
/// Order items routes
/// </summary>
module OrderItems =
    let routes<'T> =
        choose [
            GET >=> routef "/orders/%i/items" OrderItemHandlers.index
            GET >=> routef "/order-items/%i" OrderItemHandlers.show
            POST >=> routef "/orders/%i/items" OrderItemHandlers.store
            PUT >=> routef "/order-items/%i" OrderItemHandlers.update
            DELETE >=> routef "/order-items/%i" OrderItemHandlers.delete
        ]

