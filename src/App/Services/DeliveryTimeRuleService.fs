namespace App.Services

open System
open Core.Entities
open Core.Exceptions.Validation
open App.Repositories
open App.Interfaces
open Infrastructure.Core.Types

module DeliveryTimeRuleService =
    let private repo        = DeliveryTimeRuleRepository :> IRepository<DeliveryTimeRule | null>
    let private weekdayRepo = WeekdayRepository :> IRepository<Weekday | null>
    let private offdayRepo  = DeliveryTimeRuleNotAvailableWeekdayRepository :> IRepository<DeliveryTimeRuleNotAvailableWeekday | null>

    let index (filters : Condition<'P> seq) : DeliveryTimeRule seq =
        let rules = repo.get [] filters

        rules

    let show (id : int) : DeliveryTimeRule * Weekday seq =
        let rule = repo.find (id.ToString()) []

        // Get weekdays
        let condition : Condition<string> = Helpers.Database.where "delivery_time_rule_not_available_weekdays.delivery_time_rule_id" (Some (rule.id.ToString()))
        let joinCondition : Condition<string>  = Helpers.Database.where "weekdays.id" (Some "delivery_time_rule_not_available_weekdays.weekday_id")
        let join : Join<string> = Helpers.Database.innerJoin "delivery_time_rule_not_available_weekdays" joinCondition

        let weekdays = weekdayRepo.get [join] [condition]

        rule, weekdays

    let store (rule : DeliveryTimeRule) : DeliveryTimeRule =
        repo.store rule

    let update (id : int) (updatedRule : DeliveryTimeRule) : DeliveryTimeRule =
        let rule = repo.find (id.ToString()) []

        repo.update (id.ToString()) updatedRule

    let delete (id : int) : unit =
        repo.delete (id.ToString())

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

    let removeOffday (id : int) : unit =
        offdayRepo.delete (id.ToString())

    // Get delivery time rules of the storage types
    let getDeliveryRulesOfStorageTypes (storageTypes : ProductStorageType seq) : DeliveryTimeRule seq =
        let rulesIds : string array = storageTypes |> Seq.map (fun _type -> _type.delivery_time_rule_id.ToString()) |> Seq.distinct |> Array.ofSeq

        repo.get [] [ Helpers.Database.whereIn "id" rulesIds ]

    // Some helpers funs to combine delivery rules into one rule field
    let private toSameDayDeadline = fun (rule : DeliveryTimeRule) -> rule.same_day_deadline
    let private forHavingValue    = fun (deadline : Nullable<TimeOnly>) -> deadline.HasValue
    let private toTimeValue       = fun (deadline : Nullable<TimeOnly>) -> deadline.Value

    // Combine the rules into one rule, and return all offdays for the rules
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

    let getCompositeRuleForOrder =
        ProductStorageTypeService.getStorageTypesOfOrder >>
        getDeliveryRulesOfStorageTypes >>
        combineRules

    // Some helpers funs to filter timeslots based on rules
    let private forGreaterThanNow  = fun (slot : TimeSlot) -> slot.time > (TimeOnly.FromDateTime DateTime.Now)
    let private toWeekdayBoolTuble = fun (weekday : Weekday) -> weekday.code, true

    // Get valid delivery time slots, based on rules of delivery related
    // to types of products in the order.
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
                match offdaysMap.TryFind (date.DayOfWeek.ToString()) with
                | Some _ -> false
                | None -> true

        // Get valid slots
        let stValidDate : DateOnly = today.AddDays rule.in_advance_days
        let dates : DateOnly list  =
            stValidDate |> getDeliveryDates |> Set.toList |> List.filter toNoneOffdays

        // Whether to eliminate today from the list
        let deadlinePassed : bool =
            match rule.same_day_deadline.HasValue with
            | true -> nowTime > rule.same_day_deadline.Value
            | false -> true

        let initDTimes : DeliveryTimes list = // Includes today slots if deadline not passed
            match not deadlinePassed with
            | true -> [ { date = dates.Head; time_slots = todaySlots } ]
            | false -> []

        let deliveryTimes : DeliveryTimes list = // Includes initial delivery times (`initDTimes`) if `rule.in_advance_days = 0`
            match rule.in_advance_days with
            | days when days = 0 -> initDTimes @ List.map (fun (date : DateOnly) -> { date = date; time_slots = slots }) dates.Tail
            | _ -> List.map (fun (date : DateOnly) -> { date = date; time_slots = slots }) dates

        deliveryTimes

