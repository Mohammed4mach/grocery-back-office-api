namespace Http.Routes

open Giraffe
open Http.Handlers

module DeliveryTimeRule =
    let routes<'T> =
        subRoute "/delivery-rules"
            (choose [
                GET  >=> route "" >=> CustomerHandlers.index
            ])

