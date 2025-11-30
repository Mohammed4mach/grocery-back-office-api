namespace App.Services

open System
open Core.Entities
open Core.Exceptions.Validation
open App.Repositories
open App.Interfaces
open Infrastructure.Core.Types

/// <summary>
/// Module that carry services that handle the business logic regarding
/// delivery time rule resourec
/// </summary>
module DeliveryTimeRuleService =
    let private repo        = DeliveryTimeRuleRepository :> IRepository<DeliveryTimeRule | null>
    let private weekdayRepo = WeekdayRepository :> IRepository<Weekday | null>
    let private offdayRepo  = DeliveryTimeRuleNotAvailableWeekdayRepository :> IRepository<DeliveryTimeRuleNotAvailableWeekday | null>

    /// <summary>
    /// Get a collection of the resource
    /// </summary>
    /// <param name="filters">The conditions for filtering the results</param>
    /// <returns>Collection of the resource</returns>
    let index (filters : Condition<'P> seq) : DeliveryTimeRule seq =
        let rules = repo.get [] filters

        rules

    /// <summary>
    /// Get the rule based on the identifier
    /// </summary>
    /// <param name="id">Identifier of the rule</param>
    /// <returns>The rule that match for the identifier and its offdays</returns>
    let show (id : int) : DeliveryTimeRule * Weekday seq =
        let rule = repo.find (id.ToString()) []

        // Get weekdays
        let condition : Condition<string> = Helpers.Database.where "delivery_time_rule_not_available_weekdays.delivery_time_rule_id" (Some (rule.id.ToString()))
        let joinCondition : Condition<string>  = Helpers.Database.where "weekdays.id" (Some "delivery_time_rule_not_available_weekdays.weekday_id")
        let join : Join<string> = Helpers.Database.innerJoin "delivery_time_rule_not_available_weekdays" joinCondition

        let weekdays = weekdayRepo.get [join] [condition]

        rule, weekdays

    /// <summary>
    /// Store a delivery time rule
    /// </summary>
    /// <param name="rule">The delivery time rule to be stored</param>
    /// <returns>The stored delivery time rule</returns>
    let store (rule : DeliveryTimeRule) : DeliveryTimeRule =
        repo.store rule

    /// <summary>
    /// Update the rule that match for the identifier
    /// </summary>
    /// <param name="id">The identifier</param>
    /// <param name="updatedRule">The values to be updated</param>
    /// <returns>The updated rule</returns>
    let update (id : int) (updatedRule : DeliveryTimeRule) : DeliveryTimeRule =
        let rule = repo.find (id.ToString()) []

        repo.update (id.ToString()) updatedRule

    /// <summary>
    /// Delete the rule that match for the identifier
    /// </summary>
    /// <param name="id">The identifier</param>
    let delete (id : int) : unit =
        repo.delete (id.ToString())

    /// <summary>
    /// Add a weekday to the list of the not available days for a delivery rule
    /// </summary>
    /// <param name="id">The identifier of delivery time rule</param>
    /// <param name="weekdayId">The identifier of weekday</param>
    /// <returns>
    /// Return entity that represents the raleation between day and rule
    /// </returns>
    let addOffday (id : int) (weekdayId : int) : DeliveryTimeRuleNotAvailableWeekday =
        let rule    = repo.find (id.ToString()) []
        let weekday = weekdayRepo.find (weekdayId.ToString()) []

        // Check if the bond exists
        let conditions : Condition<string> seq = [
            Helpers.Database.where "delivery_time_rule_id" (Some (rule.id.ToString()))
            Helpers.Database.where "weekday_id" (Some (weekday.id.ToString()))
        ]

        let bondCount  = offdayRepo.count conditions
        let bondExists = bondCount > 0

        if bondExists then
            raise (ConflictError $"{rule.name} already has {weekday.name} in its offday list")

        let offdayBond : DeliveryTimeRuleNotAvailableWeekday =
            {
                id                    = 0
                delivery_time_rule_id = rule.id
                weekday_id            = weekday.id
            }

        offdayRepo.store offdayBond

    /// <summary>
    /// Remove weekday from the list of the not available days of a delivery rule
    /// </summary>
    /// <param name="id">The identifier of delivery time rule</param>
    /// <returns>
    /// Return entity that represents the raleation between day and rule
    /// </returns>
    let removeOffday (id : int) : unit =
        offdayRepo.delete (id.ToString())

    /// <summary>
    /// Get delivery time rules attached to the provided product storag types
    /// </summary>
    /// <param name="storageTypes">Sequence of product storage types</param>
    /// <returns>Sequence of related delivery time rules</returns>
    let getDeliveryRulesOfStorageTypes (storageTypes : ProductStorageType seq) : DeliveryTimeRule seq =
        let rulesIds : string array = storageTypes |> Seq.map (fun _type -> _type.delivery_time_rule_id.ToString()) |> Seq.distinct |> Array.ofSeq

        repo.get [] [ Helpers.Database.whereIn "id" rulesIds ]

    (* Some helpers funs to combine delivery rules into one rule field *)
    /// <summary>
    /// Extract `same_day_deadline` field from a delivery time rule
    /// </summary>
    /// <param name="rule">The delivery time rule</param>
    /// <returns>The `same_day_deadline` field of the given rule</returns>
    /// <example>
    /// <code>
    /// let deadlines = deliveryRules |> Seq.filter toSameDayDeadline
    /// </code>
    /// </example>
    let private toSameDayDeadline = fun (rule : DeliveryTimeRule) -> rule.same_day_deadline

    /// <summary>
    /// Indicates whether the `same_day_deadline` having a value. This
    /// method is dedicated for filtering the nullable deadline
    /// </summary>
    /// <param name="deadline">The `same_day_deadline` field</param>
    /// <returns>True if the field has value</returns>
    /// <example>
    /// <code>
    /// let validDeadlines = deadlines |> Seq.filter forHavingValue
    /// </code>
    /// </example>
    let private forHavingValue = fun (deadline : Nullable<TimeOnly>) -> deadline.HasValue

    /// <summary>
    /// Extract the value of the nullable `same_day_deadline`. It is recommended
    /// to be used with `forHavingValue` function to filter the nullable values
    /// </summary>
    /// <param name="deadline">The `same_day_deadline` field</param>
    /// <returns>Values of the nullable field</returns>
    /// <exception cref="System.InvalidOperationException">
    /// Thrown when the <paramref name="deadline" /> has `null` value.
    /// </exception>
    /// <example>
    /// <code>
    /// let timeOnly =
    ///     deadlines |>
    ///     Seq.filter forHavingValue |> // Recommended
    ///     Seq.map toTimeValue |>
    /// </code>
    /// </example>
    let private toTimeValue = fun (deadline : Nullable<TimeOnly>) -> deadline.Value

    /// <summary>
    /// Compose the rules into one composite rule that satisfy the delivery
    /// constraints
    /// </summary>
    /// <param name="rules">Sequence of delivery rules</param>
    /// <returns>The composite rule and its non available weekdays</returns>
    let combineRules (rules : DeliveryTimeRule seq) : DeliveryTimeRule * Weekday seq =
        let offdays : Weekday seq      = rules |> WeekdayService.getOffdaysOfDeliveryRule
        let inAdvanceDays : int        = rules |> Seq.map (fun rule -> rule.in_advance_days) |> Seq.max
        let sameDayDeadlines : TimeOnly seq =
            rules |>
            Seq.map toSameDayDeadline |>
            Seq.filter forHavingValue |>
            Seq.map toTimeValue

        let sameDayDeadline : TimeOnly option =
            match Seq.isEmpty sameDayDeadlines with
            | true -> None
            | false -> Some (Seq.min sameDayDeadlines)

        let rule : DeliveryTimeRule = {
            DeliveryTimeRule.Default with
                in_advance_days   = inAdvanceDays
                same_day_deadline =
                    match sameDayDeadline with
                    | Some deadline -> Nullable(deadline)
                    | None -> Nullable()
        }

        rule, offdays

    /// <summary>
    /// Functions composition to get a composite rule for an order.
    /// </summary>
    /// <param name="order">The order</param>
    /// <returns>The composite rule and its non available weekdays</returns>
    /// <example>
    /// <code>
    /// let compositRuel, offdays = order |> getCompositeRuleForOrder
    /// </code>
    /// </example>
    let getCompositeRuleForOrder =
        ProductStorageTypeService.getStorageTypesOfOrder >>
        getDeliveryRulesOfStorageTypes >>
        combineRules

    (* Some helpers funs to filter timeslots based on rules *)
    /// <summary>
    /// Indicates whether a given time slot is greater than now. Dedicated
    /// for filtering the time slots less than the current time
    /// </summary>
    /// <param name="slot">The time slot</param>
    /// <returns>True if the slot is greater than now</returns>
    /// <example>
    /// <code>
    /// let afterNowSlots = slots |> Set.filter forGreaterThanNow
    /// </code>
    /// </example>
    let private forGreaterThanNow  = fun (slot : TimeSlot) -> slot.time > (TimeOnly.FromDateTime DateTime.Now)

    /// <summary>
    /// Convert weekday into the tuble `weekday.code, true`. Dedicated
    /// to facilitate the process of getting map of offdays, for quick
    /// access and validation.
    /// </summary>
    /// <param name="weekday">The day of the week</param>
    /// <returns>A pair of weekday code and a `true` literal</returns>
    /// <example>
    /// <code>
    /// let offdaysMap =
    ///     offdays |>
    ///     Seq.map toWeekdayBoolTuble |>
    ///     Map.ofSeq
    /// </code>
    /// </example>
    let private toWeekdayBoolTuble = fun (weekday : Weekday) -> weekday.code.ToLower(), true

    /// <summary>
    /// Get valid delivery time slots, based on rules of delivery related
    /// to types of products in the order.
    /// </summary>
    /// <param name="order">The order</param>
    /// <param name="getDeliveryDates">
    /// A function that take the start date to generate delivery dates
    /// to a date calculated according to maximum allowed day to order in
    /// advance
    /// </param>
    /// <param name="slots">
    /// The time slots of the working-day hours
    /// </param>
    /// <returns>
    /// Sequence of valid delivery times that comply to order rules
    /// </returns>
    /// <example>
    /// <code>
    /// codeExample
    /// </code>
    /// </example>
    let getValidDeliveryTimes (order : Order) (getDeliveryDates : DateOnly -> Set<DateOnly>) (slots : Set<TimeSlot>) : DeliveryTimes seq =
        let now : DateTime     = DateTime.Now
        let today : DateOnly   = DateOnly.FromDateTime now
        let nowTime : TimeOnly = TimeOnly.FromDateTime now
        let rule, offdays      = order |> getCompositeRuleForOrder // Get the composite rule

        let offdaysMap : Map<string, bool> = offdays |> Seq.map toWeekdayBoolTuble |> Map.ofSeq
        let todaySlots : Set<TimeSlot>     = slots |> Set.filter forGreaterThanNow // All slots after the current time

        // Filter dates against offdays and `rule.in_advance_days`
        let toNoneOffdays =
            fun (date : DateOnly) ->
                match offdaysMap.TryFind (date.DayOfWeek.ToString().ToLower()) with
                | Some _ -> false
                | None -> true

        // Get valid slots
        let stValidDate : DateOnly = today.AddDays rule.in_advance_days
        let dates : DateOnly list  =
            stValidDate |>
            getDeliveryDates |>
            Set.toList |>
            List.filter toNoneOffdays

        // Whether to eliminate today from the list
        let firstIsToday   : bool = dates.Head = today
        let deadlinePassed : bool =
            match rule.same_day_deadline.HasValue with
            | true -> nowTime > rule.same_day_deadline.Value
            | false -> true

        let initDTimes : DeliveryTimes list = // Includes today slots if first valid date is today and deadline not passed
            match firstIsToday && not deadlinePassed with
            | true -> [ { date = dates.Head; time_slots = todaySlots } ]
            | false -> []

        let deliveryTimes : DeliveryTimes list = // Includes initial delivery times (`initDTimes`) if first valid date is today and `rule.in_advance_days = 0`
            match rule.in_advance_days with
            | days when firstIsToday  && days = 0 -> initDTimes @ List.map (fun (date : DateOnly) -> { date = date; time_slots = slots }) dates.Tail
            | _ -> List.map (fun (date : DateOnly) -> { date = date; time_slots = slots }) dates

        deliveryTimes

