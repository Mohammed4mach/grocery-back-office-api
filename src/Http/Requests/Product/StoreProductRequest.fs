namespace Http.Requests

open Core.Interfaces
open Core.ValidationRules

[<CLIMutable>]
type StoreProductRequest =
    {
        name                    : string
        price                   : float
        description             : string
        product_storage_type_id : int
    }

    interface IValidatable with
        member this.Rules (): IValidationRule seq =
            [
                (* name validation *)
                new Required<string>("name", this.name)
                new Strings.Min("name", this.name, 2); new Strings.Max("name", this.name, 255)
                (* price validation *)
                new Required<float>("price", this.price)
                new Numerics.Min<float>("price", this.price, 0.01); new Numerics.Max<float>("price", this.price, 9999999)
                (* description validation *)
                new Required<string>("description", this.description)
                new Strings.Min("description", this.description, 8); new Strings.Max("description", this.description, 35500)
                (* ProductStorageType exists *)
                new Exists<int>("product_storage_type_id", this.product_storage_type_id, "product_storage_types", "id")
            ]

