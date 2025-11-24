namespace Http.Resources

open Core.Entities

type OrderItemResourceData =
    {
        id: int
        cost_per_item: float
        quantity: int
        product_id: int
        order_id: int
    }

    static member ofEntity (item : OrderItem) : OrderItemResourceData =
        {
            id            = item.id
            cost_per_item = item.cost_per_item
            quantity      = item.quantity
            product_id    = item.product_id
            order_id      = item.order_id
        }

type OrderItemResource =
    {
        data : OrderItemResourceData
    }

    static member ofEntity (item : OrderItem) : OrderItemResource =
        { data = OrderItemResourceData.ofEntity item }

