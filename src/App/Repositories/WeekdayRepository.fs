namespace App.Repositories

open Core.Entities
open Infrastructure.Repositories

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

