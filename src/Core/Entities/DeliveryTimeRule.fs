namespace Core.Entities

open System

/// <summary>
/// Entity that model the delivery time rule
/// </summary>
[<CLIMutable>]
type DeliveryTimeRule =
    {
        id: int
        name: string
        in_advance_days: int
        same_day_deadline: Nullable<TimeOnly>
    }

    static member Default : DeliveryTimeRule =
        {
            id                = 0
            name              = ""
            in_advance_days   = 0
            same_day_deadline = Nullable()
        }

