namespace Http.Routes

open Giraffe
open Http.Handlers

/// <summary>
/// Weekdays routes
/// </summary>
module Weekdays =
    let routes<'T> =
        subRoute "/weekdays"
            (choose [
                GET  >=> route "" >=> WeekdayHandlers.index
                GET  >=> routef "/%i" WeekdayHandlers.show
                POST >=> route "" >=> WeekdayHandlers.store
                PUT >=> routef "/%i" WeekdayHandlers.update
                DELETE >=> routef "/%i" WeekdayHandlers.delete
            ])

