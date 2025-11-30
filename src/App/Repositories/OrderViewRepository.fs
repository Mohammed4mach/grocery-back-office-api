namespace App.Repositories

open Core.Entities
open Infrastructure.Repositories

/// <summary>
/// Order entity repository
/// </summary>
[<AutoOpen>]
module OrderRepository =
    let OrderViewRepository : Repository<OrderView | null> = {
        Repository.Default with
            table = "orders_view"
            fillable = []
    }

