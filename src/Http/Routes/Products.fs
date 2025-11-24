namespace Http.Routes

open Giraffe
open Http.Handlers

module Products =
    let routes<'T> =
        subRoute "/products"
            (choose [
                GET  >=> route "" >=> CustomerHandlers.index
            ])

