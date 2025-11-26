namespace App.Services

open Core.Entities
open App.Repositories
open App.Interfaces
open Infrastructure.Core.Types

module ProductService =
    let private repo     = ProductRepository :> IRepository<Product | null>
    let private typeRepo = ProductStorageTypeRepository :> IRepository<ProductStorageType | null>

    let index (filters : Condition seq) : Product seq =
        let products = repo.get [] filters

        products

    let show (id : int) : Product * ProductStorageType =
        let product = repo.find (id.ToString()) []

        // Get storage type
        let storageType = typeRepo.find (product.product_storage_type_id.ToString()) []

        product, storageType

    let store (product : Product) : Product =
        repo.store product

    let update (id : int) (updatedProduct : Product) : Product =
        let product = repo.find (id.ToString())

        repo.update (id.ToString()) updatedProduct

    let delete (id : int) : unit =
        repo.delete (id.ToString())

