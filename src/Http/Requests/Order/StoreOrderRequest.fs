namespace Http.Requests

open System
open Core.Interfaces
open Core.ValidationRules

[<CLIMutable>]
type OrderItemData =
    {
        quantity   : Nullable<int>
        product_id : Nullable<int>
    }

/// <summary>
/// Validatable request that validate order store data
/// </summary>
[<CLIMutable>]
type StoreOrderRequest =
    {
        mutable user_id : int
        customer_id : int
        items : OrderItemData seq
    }

    static member private getItemValidationRules (item : OrderItemData) : IValidationRule list =
        let qty =
            match item.quantity.HasValue with
            | false -> 0
            | true -> item.quantity.Value

        let productId =
            match item.product_id.HasValue with
            | false -> 0
            | true -> item.product_id.Value

        [
            (* Product validation *)
            new Exists<int>("product_id", productId, "products", "id")
            (* Order item validation *)
            new Numerics.Min<int>("quantity", qty, 1)
            new Numerics.Max<int>("quantity", qty, 9999999)
        ]

    interface IValidatable with
        member this.Rules (): IValidationRule seq =
            let items =
                match this.items with
                | null -> []
                | items -> List.ofSeq items

            let orderRules : IValidationRule list = [
                (* User exists *)
                new Required<int>("user_id", this.user_id)
                new Exists<int>("user_id", this.user_id, "users", "id")
                (* Customer exists *)
                new Required<int>("customer_id", this.customer_id);
                new Exists<int>("customer_id", this.customer_id, "customers", "id")
            ]

            let itemsRules : IValidationRule list =
                items |> List.fold (fun validations item -> validations @ StoreOrderRequest.getItemValidationRules item) []

            orderRules @ itemsRules

