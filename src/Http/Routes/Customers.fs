namespace Http.Routes

open Giraffe
open Http.Handlers

/// <summary>
/// Customers routes
/// </summary>
module Customers =
    let routes<'T> =
        subRoute "/customers"
            (choose [
                GET  >=> route "" >=> CustomerHandlers.index
                GET  >=> routef "/%i" CustomerHandlers.show
                POST >=> route "" >=> CustomerHandlers.store
                PUT >=> routef "/%i" CustomerHandlers.update
                DELETE >=> routef "/%i" CustomerHandlers.delete
            ])

