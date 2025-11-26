namespace App.Repositories

open Core.Entities
open Infrastructure.Repositories

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

