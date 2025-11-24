namespace Http.Routes

open Giraffe
open Http.Handlers

module OrderItems =
    let routes<'T> =
        subRoute "/order-items"
            (choose [
                GET  >=> route "" >=> CustomerHandlers.index
            ])

