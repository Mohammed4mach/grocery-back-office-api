namespace Http.Requests

open Core.Interfaces
open Core.ValidationRules

[<CLIMutable>]
type UpdateOrderItemRequest =
    {
        mutable id: int
        quantity: int
    }

    interface IValidatable with
        member this.Rules (): IValidationRule seq =
            [
                (* OrderItem exists *)
                new Required<int>("id", this.id);
                new Exists<int>("id", this.id, "order_items", "id")
                (* quantity validation *)
                new Required<int>("quantity", this.quantity);
                new Numerics.Min<int>("quantity", this.quantity, 1); new Numerics.Max<int>("quantity", this.quantity, 9999999)
            ]

