namespace Http.Routes

open Giraffe
open Http.Handlers

module Weekdays =
    let routes<'T> =
        subRoute "/weekdays"
            (choose [
                GET  >=> route "" >=> CustomerHandlers.index
            ])

