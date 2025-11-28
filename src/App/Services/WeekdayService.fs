namespace App.Services

open Core.Entities
open App.Repositories
open App.Interfaces
open Infrastructure.Core.Types

/// <summary>
/// Module that carry services that handle the business logic regarding weekdays resource
/// </summary>
module WeekdayService =
    let private repo = WeekdayRepository :> IRepository<Weekday | null>

    /// <summary>
    /// Get a collection of the resource
    /// </summary>
    /// <typeparam name="'Y">Conditions values type</typeparam>
    /// <param name="filters">The conditions for filtering the results</param>
    /// <returns>Collection of the resource</returns>
    let index<'Y when 'Y : null> (filters : Condition<'Y> seq) : Weekday seq =
        let weekdays = repo.get [] filters

        weekdays

    /// <summary>
    /// Get the record of the resource based on the identifier
    /// </summary>
    /// <param name="id">Identifier of the record</param>
    /// <returns>The entity that match for the identifier</returns>
    let show (id : int) : Weekday =
        let weekday = repo.find (id.ToString()) []

        weekday

    /// <summary>
    /// Store a record of the resource
    /// </summary>
    /// <param name="weekday">The weekday to be stored</param>
    /// <returns>The stored weekday</returns>
    let store (weekday : Weekday) : Weekday =
        repo.store weekday

    /// <summary>
    /// Update the record that match for the identifier
    /// </summary>
    /// <param name="id">The identifier</param>
    /// <param name="updatedWeekday">The values to be updated</param>
    /// <returns>The updated record</returns>
    let update (id : int) (updatedWeekday : Weekday) : Weekday =
        let weekday = repo.find (id.ToString()) []

        repo.update (id.ToString()) updatedWeekday

    /// <summary>
    /// Delete the record that match for the identifier
    /// </summary>
    /// <param name="id">The identifier</param>
    let delete (id : int) : unit =
        repo.delete (id.ToString())

    /// <summary>
    /// Get offdays of the provided delivery rules
    /// </summary>
    /// <param name="rules">Sequence of delivery time rules</param>
    /// <returns>
    /// Sequence of weekdays that considered not available for the rules
    /// </returns>
    let getOffdaysOfDeliveryRule (rules : DeliveryTimeRule seq) : Weekday seq =
        let rulesIds : string array = rules |> Seq.map (fun (rule : DeliveryTimeRule) -> rule.id.ToString()) |> Array.ofSeq
        let condition : Condition<string array> = Helpers.Database.whereIn "delivery_time_rule_id" rulesIds
        let join : Join<string> =
            Helpers.Database.where "weekdays.id" (Some "weekday_id") |>
            Helpers.Database.innerJoin "delivery_time_rule_not_available_weekdays"

        repo.get [ join ] [ condition ]

