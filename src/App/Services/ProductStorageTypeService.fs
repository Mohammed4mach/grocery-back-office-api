namespace App.Services

open Core.Entities
open Infrastructure.Repositories
open Infrastructure.Core.Types

module ProductStorageTypeService =
    let index (filters : Condition seq) : ProductStorageType seq =
        let productTypes = ProductStorageTypeRepository.get filters

        productTypes

    let show (id : string) : ProductStorageType =
        let productType = ProductStorageTypeRepository.find id

        productType

    let store (productType : ProductStorageType) : ProductStorageType =
        ProductStorageTypeRepository.store productType

    let update (id : string) (updatedProductStorageType : ProductStorageType) : ProductStorageType =
        let productType = ProductStorageTypeRepository.find id

        ProductStorageTypeRepository.update id updatedProductStorageType

    let delete (id : string) : unit =
        ProductStorageTypeRepository.delete id

