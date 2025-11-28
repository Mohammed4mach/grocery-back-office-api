namespace Http.Requests

open Core.Interfaces
open Core.ValidationRules

/// <summary>
/// Validatable request that validate order item store data
/// </summary>
[<CLIMutable>]
type StoreOrderItemRequest =
    {
        quantity         : int
        product_id       : int
        mutable order_id : int
    }

    interface IValidatable with
        member this.Rules (): IValidationRule seq =
            [
                (* Order exists *)
                new Required<int>("order_id", this.order_id);
                new Exists<int>("order_id", this.order_id, "orders", "id")
                (* quantity validation *)
                new Required<int>("quantity", this.quantity);
                new Numerics.Min<int>("quantity", this.quantity, 1); new Numerics.Max<int>("quantity", this.quantity, 9999999)
                (* Product exists *)
                new Required<int>("product_id", this.product_id);
                new Exists<int>("product_id", this.product_id, "products", "id")
            ]

