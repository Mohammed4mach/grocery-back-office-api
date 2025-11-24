namespace Http.Requests

open Core.Interfaces
open Core.ValidationRules

[<CLIMutable>]
type StoreOrderRequest =
    {
        customer_id : int
    }

    interface IValidatable with
        member this.Rules (): IValidationRule seq =
            [
                (* Customer exists *)
                new Required<int>("customer_id", this.customer_id);
                new Exists<int>("customer_id", this.customer_id, "customers", "id")
            ]

