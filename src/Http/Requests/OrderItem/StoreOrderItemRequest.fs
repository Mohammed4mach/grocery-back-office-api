namespace Http.Requests

open Core.Interfaces
open Core.ValidationRules

[<CLIMutable>]
type StoreOrderItemRequest =
    {
        quantity      : int
        product_id    : int
        order_id      : int
    }

    interface IValidatable with
        member this.Rules (): IValidationRule seq =
            [
                (* quantity validation *)
                new Required<int>("quantity", this.quantity);
                new Numerics.Min<int>("quantity", this.quantity, 1); new Numerics.Max<int>("quantity", this.quantity, 9999999)
                (* Order exists *)
                new Required<int>("order_id", this.order_id);
                new Exists<int>("order_id", this.order_id, "orders", "id")
                (* Product exists *)
                new Required<int>("product_id", this.product_id);
                new Exists<int>("product_id", this.product_id, "products", "id")
            ]

