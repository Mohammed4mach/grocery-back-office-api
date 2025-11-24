namespace Http.Requests

open System
open Core.Interfaces
open Core.ValidationRules

[<CLIMutable>]
type StoreDeliveryTimeRuleRequest =
    {
        name              : string
        in_advance_days   : int
        same_day_deadline : DateTime
    }

    interface IValidatable with
        member this.Rules (): IValidationRule seq =
            [
                (* name validation *)
                new Required<string>("name", this.name)
                new Strings.Min("name", this.name, 2); new Strings.Max("name", this.name, 255)
                (* in_advance_days validation *)
                new Required<int>("in_advance_days", this.in_advance_days)
                new Numerics.Min<int>("in_advance_days", this.in_advance_days, 0); new Numerics.Max<int>("in_advance_days", this.in_advance_days, 365)
            ]

