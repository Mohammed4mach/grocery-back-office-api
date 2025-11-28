namespace App.Services

open System
open Core.Entities

/// <summary>
/// Module that have business logic of delivery times
/// </summary>
module DeliveryTimeService =

    /// <summary>
    /// Get delivery dates from start date to maximum allowed days to
    /// schedule delivery on advance
    /// </summary>
    /// <param name="startDate">The start date to begin generation of dates</param>
    /// <returns>
    /// Set of dates from <paramref startDate` to `startDate` + max days to order
    /// in advance
    /// </returns>
    let getDeliveryDates (startDate : DateOnly) : Set<DateOnly> =
        let maxDaysInAdvance : int    = 14
        let maxDaysWithoutToday : int = maxDaysInAdvance - 1

        seq { for i in 0..maxDaysWithoutToday -> startDate.AddDays i } |> Set.ofSeq

    /// <summary>
    /// Indicates whether the <paramref name="time" /> is a green time slot
    /// </summary>
    /// <param name="time">The time slot</param>
    /// <returns>Returns true if the <paramref name="time" /> is a green time slot</returns>
    let isGreenTime (time : TimeOnly) : bool =
        let offpeakSlots : Map<string, bool> =
            [
                "10:00", true
                "11:00", true
                "12:00", true
            ] |> Map.ofList

        let timeStr : string = time.ToString "HH:mm"

        match offpeakSlots.TryFind timeStr with
        | Some bool -> bool
        | None -> false

    /// <summary>
    /// Get 1 hour time slots for daily working hours
    /// </summary>
    /// <returns>Set of timeslots between (start - end) of working hours</returns>
    let getTimeSlots() : Set<TimeSlot> =
        let startSlot    : TimeOnly = TimeOnly.Parse "08:00"
        let endSlot      : TimeOnly = TimeOnly.Parse "22:00"
        let workingHrs   : int      = (endSlot - startSlot).Hours

        seq {
            for i in 0..workingHrs ->
                let slotTime = startSlot.AddHours (float i)
                let isGreen  = isGreenTime slotTime

                {
                    time     = slotTime
                    is_green = isGreen
                }
            } |> Set.ofSeq

    /// <summary>
    /// Get valid delivery time slots for specific order
    /// </summary>
    /// <param name="order">The order</param>
    /// <returns>
    /// Sequence of valid delivery time based on rules related to order's
    /// products types
    /// </returns>
    let getDeliveryTimes (order : Order) : DeliveryTimes seq =
        let timeSlots     : Set<TimeSlot>     = getTimeSlots()
        let deliveryTimes : DeliveryTimes seq = DeliveryTimeRuleService.getValidDeliveryTimes order getDeliveryDates timeSlots

        deliveryTimes

    /// <summary>
    /// Indicates whether the delivery time is valid for delivery rules
    /// that apply on the order's products
    /// </summary>
    /// <param name="order">The order</param>
    /// <param name="deliveryTime">The choosen delivery time</param>
    /// <returns>
    /// Returns true if <paramref name="deliveryTime" /> is included in
    /// the valid delivery times for the <paramref name="order" />
    /// </returns>
    let isValidDeliveryTime (order : Order) (deliveryTime : DeliveryTime) : bool =
        let validSlots : DeliveryTimes seq = getDeliveryTimes order

        let validDTimes : DeliveryTimes option = validSlots |> Seq.tryFind (fun (time : DeliveryTimes) -> time.date = deliveryTime.date )
        let isValid : bool =
            match validDTimes with
            | Some dTimes -> dTimes.time_slots |> Set.exists (fun (slot : TimeSlot) -> slot.time = deliveryTime.time)
            | None -> false

        isValid

