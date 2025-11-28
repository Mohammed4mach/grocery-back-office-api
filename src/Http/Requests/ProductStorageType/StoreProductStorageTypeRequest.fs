namespace Http.Requests

open Core.Interfaces
open Core.ValidationRules

/// <summary>
/// Validatable request that validate product storage type store data
/// </summary>
[<CLIMutable>]
type StoreProductStorageTypeRequest =
    {
        name                  : string
        delivery_time_rule_id : int
    }

    interface IValidatable with
        member this.Rules (): IValidationRule seq =
            [
                (* name validation *)
                new Required<string>("name", this.name)
                new Strings.Min("name", this.name, 2); new Strings.Max("name", this.name, 255)
                (* Rule exists *)
                new Required<int>("delivery_time_rule_id", this.delivery_time_rule_id)
                new Exists<int>("delivery_time_rule_id", this.delivery_time_rule_id, "delivery_time_rules", "id")
            ]

