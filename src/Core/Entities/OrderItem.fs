namespace Core.Entities

[<CLIMutable>]
type OrderItem = {
    id: int
    cost_per_item: float
    quantity: int
    product_id: int
    order_id: int
}

