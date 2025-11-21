namespace Core.Entities

[<CLIMutable>]
type OrderItem = {
    id: int
    costPerItem: float
    quantity: int
    productId: int
    orderId: int
}

