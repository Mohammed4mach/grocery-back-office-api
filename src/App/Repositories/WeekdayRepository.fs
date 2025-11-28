namespace App.Repositories

open Core.Entities
open Infrastructure.Repositories

/// <summary>
/// Weekday entity repository
/// </summary>
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

