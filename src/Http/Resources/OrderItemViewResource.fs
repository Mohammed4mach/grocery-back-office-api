namespace Http.Resources

open Core.Entities

type OrderItemViewResourceData =
    {
        id: int
        cost_per_item: float
        quantity: int
        product_id: int
        order_id: int
        product_name: string
    }

    static member ofEntity (item : OrderItemView) : OrderItemViewResourceData =
        {
            id            = item.id
            cost_per_item = item.cost_per_item
            quantity      = item.quantity
            product_id    = item.product_id
            order_id      = item.order_id
            product_name    = item.product_name
        }

type OrderItemViewResource =
    {
        data : OrderItemViewResourceData
    }

    static member ofEntity (item : OrderItemView) : OrderItemViewResource =
        { data = OrderItemViewResourceData.ofEntity item }

