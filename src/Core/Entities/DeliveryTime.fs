namespace Core.Entities

open System

[<CLIMutable>]
type DeliveryTime = {
    date : DateOnly
    time_slots : Set<TimeSlot>
}

