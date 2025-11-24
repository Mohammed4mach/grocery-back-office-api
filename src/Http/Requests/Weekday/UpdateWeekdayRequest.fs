namespace Http.Requests

open Core.Interfaces
open Core.ValidationRules

[<CLIMutable>]
type UpdateWeekdayRequest =
    {
        mutable id : int
        name       : string
        code       : string
    }

    interface IValidatable with
        member this.Rules (): IValidationRule seq =
            [
                (* Weekday exists *)
                new Exists<int>("id", this.id, "weekdays", "id")
                (* name validation *)
                new Required<string>("name", this.name)
                new Strings.Min("name", this.name, 2); new Strings.Max("name", this.name, 255)
                new UniqueIgnore<string, int>("name", this.name, "weekdays", "name", "id", this.id)
                (* code validation *)
                new Required<string>("code", this.code)
                new Strings.Min("code", this.code, 2); new Strings.Max("code", this.code, 255)
                new UniqueIgnore<string, int>("code", this.code, "weekdays", "code", "id", this.id)
            ]

