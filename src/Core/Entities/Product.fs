namespace Core.Entities

[<CLIMutable>]
type Product = {
    id: int
    name: string
    price: float
    description: string
    productStorageTypeId: int
}

