namespace Http.Routes

open Giraffe
open Http.Handlers

module ProductStorageTypes =
    let routes<'T> =
        subRoute "/product-storage-types"
            (choose [
                GET  >=> route "" >=> ProductStorageTypeHandlers.index
                GET  >=> routef "/%i" ProductStorageTypeHandlers.show
                POST >=> route "" >=> ProductStorageTypeHandlers.store
                PUT >=> routef "/%i" ProductStorageTypeHandlers.update
                DELETE >=> routef "/%i" ProductStorageTypeHandlers.delete
            ])

