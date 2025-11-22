namespace Infrastructure.Repositories

open Core.Entities

[<AutoOpen>]
module Product =
    let ProductRepository : Repository<Product> = {
        Repository.Default with
            table = "products"
            fillable = [
                "id"
                "name"
                "price"
                "description"
                "product_storage_type_id"
            ]
    }

