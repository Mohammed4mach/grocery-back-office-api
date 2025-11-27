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

    // Get weekdays not available for delivery for the delivery time rules
    let getOffdaysOfDeliveryRule (rules : DeliveryTimeRule seq) : Weekday seq =
        let rulesIds : string array = rules |> Seq.map (fun (rule : DeliveryTimeRule) -> rule.id.ToString()) |> Array.ofSeq
        let condition : Condition<string array> = Helpers.Database.whereIn "delivery_time_rule_id" rulesIds
        let join : Join<string> =
            Helpers.Database.where "weekdays.id" (Some "weekday_id") |>
            Helpers.Database.innerJoin "delivery_time_rule_not_available_weekdays"

        repo.get [ join ] [ condition ]

