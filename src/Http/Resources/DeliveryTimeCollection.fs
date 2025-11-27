namespace Http.Resources

open System
open Core.Entities

type DeliveryTimeResourceData =
    {
        date : DateOnly
        time_slots : TimeSlot seq
    }

    static member ofEntity (time : DeliveryTime) : DeliveryTimeResourceData =
        {
            date       = time.date
            time_slots = time.time_slots
        }

type DeliveryTimeCollection =
    {
        data : DeliveryTimeResourceData seq
    }

    static member ofEntity (times : DeliveryTime seq) : DeliveryTimeCollection =
        let timesResource : DeliveryTimeResourceData seq = times |> Seq.map DeliveryTimeResourceData.ofEntity

        { data = timesResource }

