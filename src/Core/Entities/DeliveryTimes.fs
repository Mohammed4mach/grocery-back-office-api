namespace Core.Entities

open System

/// <summary>
/// Entity that model delivery time slots
/// </summary>
[<CLIMutable>]
type DeliveryTimes = {
    date : DateOnly
    time_slots : Set<TimeSlot>
}

