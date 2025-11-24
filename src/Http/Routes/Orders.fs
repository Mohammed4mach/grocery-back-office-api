namespace Http.Routes

open Giraffe
open Http.Handlers

module Orders =
    let routes<'T> =
        subRoute "/orders"
            (choose [
                GET  >=> route "" >=> CustomerHandlers.index
            ])

