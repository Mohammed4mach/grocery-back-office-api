namespace App.Repositories

open Core.Entities
open Infrastructure.Repositories

/// <summary>
/// Product entity repository
/// </summary>
[<AutoOpen>]
module ProductView =
    let ProductViewRepository : Repository<ProductView | null> = {
        Repository.Default with
            table = "products_view"
            fillable = []
    }

