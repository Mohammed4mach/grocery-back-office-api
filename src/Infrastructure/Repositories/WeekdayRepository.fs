namespace Infrastructure.Repositories

open Core.Entities

[<AutoOpen>]
module Weekday =
    let WeekdayRepository : Repository<Weekday> = {
        Repository.Default with
            table = "weekdays"
            fillable = [
                "id"
                "name"
                "code"
            ]
    }

