namespace Core.Entities

/// <summary>
/// Entity that model the order item
/// </summary>
[<CLIMutable>]
type OrderItemView =
    {
        id: int
        cost_per_item: float
        quantity: int
        product_id: int
        order_id: int
        product_name: string
    }

