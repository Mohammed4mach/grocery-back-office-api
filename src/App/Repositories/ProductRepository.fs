namespace App.Repositories

open Core.Entities
open Infrastructure.Repositories

/// <summary>
/// Product entity repository
/// </summary>
[<AutoOpen>]
module Product =
    let ProductRepository : Repository<Product | null> = {
        Repository.Default with
            table = "products"
            fillable = [
                "name"
                "price"
                "description"
                "product_storage_type_id"
            ]
    }

