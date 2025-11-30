namespace Core.Entities

/// <summary>
/// Entity that model products resource view
/// </summary>
[<CLIMutable>]
type ProductView = {
    id: int
    name: string
    price: float
    description: string | null
    product_storage_type_id: int
    product_storage_type_name: string
}

