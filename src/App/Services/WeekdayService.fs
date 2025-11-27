namespace App.Services

open Core.Entities
open App.Repositories
open App.Interfaces
open Infrastructure.Core.Types

module WeekdayService =
    let private repo = WeekdayRepository :> IRepository<Weekday | null>

    let index<'Y when 'Y : null> (filters : Condition<'Y> seq) : Weekday seq =
        let weekdays = repo.get [] filters

        weekdays

    let show (id : int) : Weekday =
        let weekday = repo.find (id.ToString()) []

        weekday

    let store (weekday : Weekday) : Weekday =
        repo.store weekday

    let update (id : int) (updatedWeekday : Weekday) : Weekday =
        let weekday = repo.find (id.ToString()) []

        repo.update (id.ToString()) updatedWeekday

    let delete (id : int) : unit =
        repo.delete (id.ToString())

