namespace Http.Routes

open Giraffe
open Http.Handlers

module ProductStorageTypes =
    let routes<'T> =
        subRoute "/product-storage-types"
            (choose [
                GET  >=> route "" >=> CustomerHandlers.index
            ])

