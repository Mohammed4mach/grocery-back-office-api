namespace App.Services

open Core.Entities
open Infrastructure.Repositories
open Infrastructure.Core.Types

module ProductStorageTypeService =
    let index (filters : Condition seq) : ProductStorageType seq =
        let productTypes = ProductStorageTypeRepository.get filters

        productTypes

    let show (id : int) : ProductStorageType =
        let productType = ProductStorageTypeRepository.find (id.ToString())

        productType

    let store (productType : ProductStorageType) : ProductStorageType =
        ProductStorageTypeRepository.store productType

    let update (id : int) (updatedProductStorageType : ProductStorageType) : ProductStorageType =
        let productType = ProductStorageTypeRepository.find (id.ToString())

        ProductStorageTypeRepository.update (id.ToString()) updatedProductStorageType

    let delete (id : int) : unit =
        ProductStorageTypeRepository.delete (id.ToString())

