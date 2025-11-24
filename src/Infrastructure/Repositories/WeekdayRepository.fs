namespace Infrastructure.Repositories

open Core.Entities

[<AutoOpen>]
module Weekday =
    let WeekdayRepository : Repository<Weekday | null> = {
        Repository.Default with
            table = "weekdays"
            fillable = [
                "name"
                "code"
            ]
    }

