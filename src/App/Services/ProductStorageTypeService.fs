namespace App.Services

open Core.Entities
open App.Repositories
open App.Interfaces
open Infrastructure.Core.Types

module ProductStorageTypeService =
    let private repo = ProductStorageTypeRepository :> IRepository<ProductStorageType | null>

    let index<'Y when 'Y : null> (filters : Condition<'Y> seq) : ProductStorageType seq =
        let productTypes = repo.get [] filters

        productTypes

    let show (id : int) : ProductStorageType =
        let productType = repo.find (id.ToString()) []

        productType

    let store (productType : ProductStorageType) : ProductStorageType =
        repo.store productType

    let update (id : int) (updatedProductStorageType : ProductStorageType) : ProductStorageType =
        let productType = repo.find (id.ToString())

        repo.update (id.ToString()) updatedProductStorageType

    let delete (id : int) : unit =
        repo.delete (id.ToString())

    // Get product storage types for all products included in an order
    let getStorageTypesOfOrder (order : Order) : ProductStorageType seq =
        let joins : Join<string> seq = [
            Helpers.Database.innerJoin "products" (Helpers.Database.where "product_storage_types.id" (Some "products.product_storage_type_id"))
            Helpers.Database.innerJoin "order_items" (Helpers.Database.where "products.id" (Some "order_items.product_id"))
        ]
        let conditions : Condition<string> seq = [ Helpers.Database.where "order_id" (Some (order.id.ToString())) ]

        repo.get joins conditions


