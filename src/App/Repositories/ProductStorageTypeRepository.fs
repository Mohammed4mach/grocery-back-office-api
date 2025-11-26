namespace App.Repositories

open Core.Entities
open Infrastructure.Repositories

[<AutoOpen>]
module ProductStorageType =
    let ProductStorageTypeRepository : Repository<ProductStorageType | null> = {
        Repository.Default with
            table = "product_storage_types"
            fillable = [
                "name"
                "delivery_time_rule_id"
            ]
    }

