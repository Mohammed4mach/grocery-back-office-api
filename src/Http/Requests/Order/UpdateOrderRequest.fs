namespace Http.Requests

open Core.Interfaces
open Core.ValidationRules

[<CLIMutable>]
type UpdateOrderRequest =
    {
        delivery_date: string | null
        delivery_time: string | null
    }

    interface IValidatable with
        member this.Rules (): IValidationRule seq =
            [
                (* delivery_date validation *)
                new Required<string>("delivery_date", this.delivery_date)
                new MatchFormat<string>("delivery_date", this.delivery_date, @"\d{4}-\d{2}-\d{2}")
                (* delivery_time validation *)
                new Required<string>("delivery_time", this.delivery_time)
                new MatchFormat<string>("delivery_time", this.delivery_time, @"\d{2}:\d{2}:\d{2}")
            ]

