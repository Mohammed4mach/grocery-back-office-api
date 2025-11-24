namespace Infrastructure.Repositories

open Core.Entities

[<AutoOpen>]
module OrderItem =
    let OrderItemRepository : Repository<OrderItem | null> = {
        Repository.Default with
            table = "order_items"
            fillable = [
                "id"
                "cost_per_item"
                "quantity"
                "product_id"
                "order_id"
            ]
    }

