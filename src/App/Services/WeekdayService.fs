namespace App.Services

open Core.Entities
open Infrastructure.Repositories
open Infrastructure.Core.Types

module WeekdayService =
    let index (filters : Condition seq) : Weekday seq =
        let weekdays = WeekdayRepository.get [] filters

        weekdays

    let show (id : int) : Weekday =
        let weekday = WeekdayRepository.find (id.ToString()) []

        weekday

    let store (weekday : Weekday) : Weekday =
        WeekdayRepository.store weekday

    let update (id : int) (updatedWeekday : Weekday) : Weekday =
        let weekday = WeekdayRepository.find (id.ToString()) []

        WeekdayRepository.update (id.ToString()) updatedWeekday

    let delete (id : int) : unit =
        WeekdayRepository.delete (id.ToString())

