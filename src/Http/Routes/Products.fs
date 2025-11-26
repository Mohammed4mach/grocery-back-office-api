namespace Http.Routes

open Giraffe
open Http.Handlers

module Products =
    let routes<'T> =
        subRoute "/products"
            (choose [
                GET  >=> route "" >=> ProductHandlers.index
                GET  >=> routef "/%i" ProductHandlers.show
                POST >=> route "" >=> ProductHandlers.store
                PUT >=> routef "/%i" ProductHandlers.update
                DELETE >=> routef "/%i" ProductHandlers.delete
            ])

