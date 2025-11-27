namespace Http.Requests

open System
open Core.Interfaces
open Core.ValidationRules

[<CLIMutable>]
type StoreWeekdayRequest =
    {
        name : string
        code : string
    }

    interface IValidatable with
        member this.Rules (): IValidationRule seq =
            [
                (* name validation *)
                new Required<string>("name", this.name)
                new Strings.Min("name", this.name, 2); new Strings.Max("name", this.name, 255)
                new Unique<string>("name", this.name, "weekdays", "name")
                (* code validation *)
                new Required<string>("code", this.code)
                new Strings.Min("code", this.code, 2); new Strings.Max("code", this.code, 255)
                new Unique<string>("code", this.code, "weekdays", "code")
            ]

