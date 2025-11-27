namespace Http.Routes

open Giraffe
open Http.Handlers

module Orders =
    let routes<'T> =
        subRoute "/orders"
            (choose [
                GET  >=> route "" >=> OrderHandlers.index
                GET  >=> routef "/%i" OrderHandlers.show
                GET >=> routef "/%i/delivery-times" OrderHandlers.getDeliveryTimes
                POST >=> route "" >=> OrderHandlers.store
                PUT >=> routef "/%i" OrderHandlers.update
                DELETE >=> routef "/%i" OrderHandlers.delete
            ])

