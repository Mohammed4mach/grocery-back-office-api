namespace Http.Requests

open System
open Core.Interfaces
open Core.ValidationRules

[<CLIMutable>]
type UpdateDeliveryTimeRuleRequest =
    {
        mutable id        : int
        name              : string
        in_advance_days   : int
        same_day_deadline : string | null
    }

    interface IValidatable with
        member this.Rules (): IValidationRule seq =
            let sameDayDeadline =
                match Some this.same_day_deadline with
                | Some deadline -> deadline
                | None -> ""

            [
                (* DeliveryTimeRule exists *)
                new Exists<int>("id", this.id, "delivery_time_rules", "id")
                (* name validation *)
                new Required<string>("name", this.name)
                new Strings.Min("name", this.name, 2); new Strings.Max("name", this.name, 255)
                (* in_advance_days validation *)
                new Required<int>("in_advance_days", this.in_advance_days)
                new Numerics.Min<int>("in_advance_days", this.in_advance_days, 0); new Numerics.Max<int>("in_advance_days", this.in_advance_days, 365)
                (* same_day_deadline validation *)
                new MatchFormat<string>("same_day_deadline", sameDayDeadline, @"\d{2}:\d{2}:\d{2}")
            ]

