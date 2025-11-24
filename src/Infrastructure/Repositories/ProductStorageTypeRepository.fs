namespace Infrastructure.Repositories

open Core.Entities

[<AutoOpen>]
module ProductStorageType =
    let ProductStorageTypeRepository : Repository<ProductStorageType | null> = {
        Repository.Default with
            table = "product_storage_types"
            fillable = [
                "id"
                "name"
                "delivery_time_rule_id"
            ]
    }

