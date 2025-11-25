namespace Core.Entities

[<CLIMutable>]
type Product = {
    id: int
    name: string
    price: float
    description: string option
    product_storage_type_id: int
}

