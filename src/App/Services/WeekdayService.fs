namespace App.Services

open Core.Entities
open Infrastructure.Repositories
open Infrastructure.Core.Types

module WeekdayService =
    let index (filters : Condition seq) : Weekday seq =
        let weekdays = WeekdayRepository.get filters

        weekdays

    let show (id : string) : Weekday =
        let weekday = WeekdayRepository.find id

        weekday

    let store (weekday : Weekday) : unit =
        WeekdayRepository.store weekday

    let update (id : string) (updatedWeekday : Weekday) : unit =
        let weekday = WeekdayRepository.find id

        WeekdayRepository.update id updatedWeekday

    let delete (id : string) : unit =
        WeekdayRepository.delete id

