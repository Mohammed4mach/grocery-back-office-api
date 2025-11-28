namespace Core.Entities

/// <summary>
/// Entity that model the order item
/// </summary>
[<CLIMutable>]
type OrderItem =
    {
        id: int
        cost_per_item: float
        quantity: int
        product_id: int
        order_id: int
    }

    static member Default : OrderItem =
        {
            id            = 0
            cost_per_item = 0.00
            quantity      = 0
            product_id    = 0
            order_id      = 0
        }

