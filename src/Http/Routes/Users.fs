namespace Http.Routes

open Giraffe
open Http.Handlers

/// <summary>
/// Users routes
/// </summary>
module Users =
    let routes<'T> =
        subRoute "/users"
            (choose [
                GET  >=> route "" >=> UserHandlers.index
                GET  >=> route "/me" >=> UserHandlers.show
                POST >=> route "" >=> UserHandlers.store
                PUT >=> route "/me" >=> UserHandlers.update
                PATCH >=> route "/me/credentials" >=> UserHandlers.updateCredentials
                DELETE >=> routef "/%i" UserHandlers.delete
            ])

