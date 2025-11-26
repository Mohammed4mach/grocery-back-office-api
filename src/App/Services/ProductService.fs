namespace App.Services

open Core.Entities
open Infrastructure.Repositories
open Infrastructure.Core.Types

module ProductService =
    let index (filters : Condition seq) : Product seq =
        let products = ProductRepository.get [] filters

        products

    let show (id : int) : Product * ProductStorageType =
        let product = ProductRepository.find (id.ToString()) []

        // Get storage type
        let storageType = ProductStorageTypeRepository.find (product.product_storage_type_id.ToString()) []

        product, storageType

    let store (product : Product) : Product =
        ProductRepository.store product

    let update (id : int) (updatedProduct : Product) : Product =
        let product = ProductRepository.find (id.ToString())

        ProductRepository.update (id.ToString()) updatedProduct

    let delete (id : int) : unit =
        ProductRepository.delete (id.ToString())

