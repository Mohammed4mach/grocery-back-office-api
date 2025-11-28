namespace Core.Entities

/// <summary>
/// Entity that model products resource
/// </summary>
[<CLIMutable>]
type Product = {
    id: int
    name: string
    price: float
    description: string | null
    product_storage_type_id: int
}

