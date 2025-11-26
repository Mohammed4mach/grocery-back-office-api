namespace Http.Requests

open System
open Core.Interfaces
open Core.ValidationRules

[<CLIMutable>]
type UpdateProductRequest =
    {
        mutable id              : int
        name                    : string
        price                   : float
        description             : string | null
        product_storage_type_id : Nullable<int>
    }

    interface IValidatable with
        member this.Rules (): IValidationRule seq =
            let description =
                match this.description with
                | null -> ""
                | desc -> desc

            let typeId =
                match this.product_storage_type_id.HasValue with
                | false -> ""
                | true -> this.product_storage_type_id.Value.ToString()

            [
                (* Product exists *)
                new Exists<int>("id", this.id, "products", "id")
                (* name validation *)
                new Required<string>("name", this.name)
                new Strings.Min("name", this.name, 2); new Strings.Max("name", this.name, 255)
                (* price validation *)
                new Required<float>("price", this.price)
                new Numerics.Min<float>("price", this.price, 0.01); new Numerics.Max<float>("price", this.price, 9999999)
                (* description validation *)
                new Strings.Max("description", description, 35500)
                (* ProductStorageType exists *)
                new Required<string>("product_storage_type_id", typeId)
                new Exists<string>("product_storage_type_id", typeId, "product_storage_types", "id")
            ]

