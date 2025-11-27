namespace App.Services

open System
open Core.Entities

module DeliveryTimeService =
    // Get delivery dates from start date to maximum allowed days to
    // schedule delivery on advance
    let getDeliveryDates (startDate : DateOnly) : Set<DateOnly> =
        let maxDaysInAdvance : int    = 14
        let maxDaysWithoutToday : int = maxDaysInAdvance - 1

        seq { for i in 0..maxDaysWithoutToday -> startDate.AddDays i } |> Set.ofSeq

    // Get 1 hour time slots for daily working hours
    let getTimeSlots() : Set<TimeSlot> =
        let offpeakSlots : Map<string, bool> = [ "10:00", true; "11:00", true; "12:00", true ] |> Map.ofList
        let startSlot    : TimeOnly = TimeOnly.Parse "08:00"
        let endSlot      : TimeOnly = TimeOnly.Parse "22:00"
        let workingHrs   : int      = (endSlot - startSlot).Hours

        seq {
            for i in 0..workingHrs ->
                let slotTime = startSlot.AddHours (float i)
                let timeStr  = slotTime.ToString "HH:mm"
                let isGreen  =
                    match offpeakSlots.TryFind timeStr with
                    | Some bool -> bool
                    | None -> false

                {
                    time     = slotTime
                    is_green = isGreen
                }
            } |> Set.ofSeq

    // Some helpers funs to filter timeslots based on rules
    let private forGreaterThanNow  = fun (slot : TimeSlot) -> slot.time > (TimeOnly.FromDateTime DateTime.Now)
    let private toWeekdayBoolTuble = fun (weekday : Weekday) -> weekday.code, true

    let private getCombinedRuleForOrder =
        ProductStorageTypeService.getStorageTypesOfOrder >>
        DeliveryTimeRuleService.getDeliveryRulesOfStorageTypes >>
        DeliveryTimeRuleService.combineRules

    // Get valid delivery times for specific order
    let getDeliveryTimes (order : Order) : DeliveryTime seq =
        let now : DateTime     = DateTime.Now
        let today : DateOnly   = DateOnly.FromDateTime now
        let nowTime : TimeOnly = TimeOnly.FromDateTime now
        let rule, offdays      = order |> getCombinedRuleForOrder
        let offdaysMap : Map<string, bool> = offdays |> Seq.map toWeekdayBoolTuble |> Map.ofSeq

        // Filter dates against offdays and `rule.in_advance_days`
        let toNoneOffdays =
            fun (date : DateOnly) ->
                match offdaysMap.TryFind (date.DayOfWeek.ToString()) with
                | Some _ -> false
                | None -> true

        let stValidDate : DateOnly = today.AddDays rule.in_advance_days
        let dates : DateOnly list  =
            stValidDate |> getDeliveryDates |> Set.toList |> List.filter toNoneOffdays

        // Filter slots based on the combined rule info
        let slots : Set<TimeSlot>      = getTimeSlots()
        let todaySlots : Set<TimeSlot> = slots |> Set.filter forGreaterThanNow

        // Whether to eliminate today from the list
        let deadlinePassed : bool =
            match rule.same_day_deadline.HasValue with
            | true -> nowTime > rule.same_day_deadline.Value
            | false -> true

        let initDTimes : DeliveryTime list = // Includes today slots if deadline not passed
            match not deadlinePassed with
            | true -> [ { date = dates.Head; time_slots = todaySlots } ]
            | false -> []

        let deliveryTimes : DeliveryTime list = // Includes initial delivery times (`initDTimes`) if `rule.in_advance_days = 0`
            match rule.in_advance_days with
            | days when days = 0 -> initDTimes @ List.map (fun (date : DateOnly) -> { date = date; time_slots = slots }) dates.Tail
            | _ -> List.map (fun (date : DateOnly) -> { date = date; time_slots = slots }) dates

        deliveryTimes

