namespace Http.Requests

open System
open Core.Interfaces
open Core.ValidationRules

[<CLIMutable>]
type UpdateOrderRequest =
    {
        mutable id: int
        delivery_date: Nullable<DateOnly>
        delivery_time: Nullable<TimeOnly>
    }

    interface IValidatable with
        member this.Rules (): IValidationRule seq =
            let dDate, dateObj =
                match this.delivery_date.HasValue with
                | false -> "", new DateOnly()
                | true ->
                    let dDate = this.delivery_date.Value.ToString()
                    let dateObj = DateOnly.Parse(dDate)

                    dDate, dateObj

            let dTime =
                match this.delivery_time.HasValue with
                | false -> ""
                | true -> this.delivery_time.Value.ToString()

            let today = DateOnly.FromDateTime(DateTime.Now)

            [
                (* Order exists *)
                new Exists<int>("id", this.id, "orders", "id")
                (* delivery_date validation *)
                new Required<string>("delivery_date", dDate)
                new MatchFormat<string>("delivery_date", dDate, @"\d{4}-\d{2}-\d{2}")
                new Dates.Min("delivery_date", dateObj, today)
                (* delivery_time validation *)
                new Required<string>("delivery_time", dTime)
                new MatchFormat<string>("delivery_time", dTime, @"\d{2}:\d{2}:\d{2}")
            ]

