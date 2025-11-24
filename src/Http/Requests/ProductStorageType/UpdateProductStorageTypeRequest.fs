namespace Http.Requests

open Core.Interfaces
open Core.ValidationRules

[<CLIMutable>]
type UpdateProductStorageTypeRequest =
    {
        mutable id            : int
        name                  : string
        delivery_time_rule_id : int
    }

    interface IValidatable with
        member this.Rules (): IValidationRule seq =
            [
                (* User exists *)
                new Exists<int>("id", this.id, "users", "id")
                (* name validation *)
                new Required<string>("name", this.name)
                new Strings.Min("name", this.name, 2); new Strings.Max("name", this.name, 255)
                (* Rule exists *)
                new Exists<int>("delivery_time_rule_id", this.delivery_time_rule_id, "delivery_time_rules", "id")
            ]

