namespace App.Repositories

open Core.Entities
open Infrastructure.Repositories

[<AutoOpen>]
module Order =
    let OrderRepository : Repository<Order | null> = {
        Repository.Default with
            table = "orders"
            fillable = [
                "total_cost"
                "order_time"
                "delivery_date"
                "delivery_time"
                "is_green_delivery"
                "user_id"
                "customer_id"
            ]
    }

