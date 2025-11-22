namespace Infrastructure.Repositories

open Core.Entities

[<AutoOpen>]
module Order =
    let OrderRepository : Repository<Order> = {
        Repository.Default with
            table = "orders"
            fillable = [
                "id"
                "total_cost"
                "order_time"
                "delivery_date"
                "delivery_time"
            ]
    }

