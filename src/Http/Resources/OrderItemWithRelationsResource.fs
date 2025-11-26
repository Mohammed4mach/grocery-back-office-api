namespace Http.Resources

open Core.Entities

type OrderItemWithRelationsResourceData =
    {
        id: int
        cost_per_item: float
        quantity: int
        product_id: int
        order_id: int
        mutable product : Product
    }

    static member ofEntity (item : OrderItem) (product : Product) : OrderItemWithRelationsResourceData =
        {
            id            = item.id
            cost_per_item = item.cost_per_item
            quantity      = item.quantity
            product_id    = item.product_id
            order_id      = item.order_id
            product       = product
        }

type OrderItemWithRelationsResource =
    {
        data : OrderItemWithRelationsResourceData
    }

    static member ofEntity (item : OrderItem) (product : Product) : OrderItemWithRelationsResource =
        { data = OrderItemWithRelationsResourceData.ofEntity item product }

