namespace Http.Requests

open System
open Core.Interfaces
open Core.ValidationRules

[<CLIMutable>]
type UpdateOrderRequest =
    {
        mutable id: int
        delivery_date: DateOnly option
        delivery_time: TimeOnly option
    }

    interface IValidatable with
        member this.Rules (): IValidationRule seq =
            [
                (* Order exists *)
                new Exists<int>("id", this.id, "orders", "id")
                (* delivery_date validation *)
                new Required<DateOnly option>("delivery_date", this.delivery_date)
                (* delivery_time validation *)
                new Required<TimeOnly option>("delivery_time", this.delivery_time)
            ]

