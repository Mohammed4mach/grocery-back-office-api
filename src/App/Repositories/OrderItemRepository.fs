namespace App.Repositories

open Core.Entities
open Infrastructure.Repositories

[<AutoOpen>]
module OrderItem =
    let OrderItemRepository : Repository<OrderItem | null> = {
        Repository.Default with
            table = "order_items"
            fillable = [
                "cost_per_item"
                "quantity"
                "product_id"
                "order_id"
            ]
    }

