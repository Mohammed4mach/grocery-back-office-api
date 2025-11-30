namespace App.Repositories

open Core.Entities
open Infrastructure.Repositories

/// <summary>
/// Order item entity repository
/// </summary>
[<AutoOpen>]
module OrderItemView =
    let OrderItemViewRepository : Repository<OrderItemView | null> = {
        Repository.Default with
            table = "order_items_view"
            fillable = []
    }

