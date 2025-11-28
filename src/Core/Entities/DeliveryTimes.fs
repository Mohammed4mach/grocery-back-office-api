namespace Core.Entities

open System

[<CLIMutable>]
type DeliveryTimes = {
    date : DateOnly
    time_slots : Set<TimeSlot>
}

