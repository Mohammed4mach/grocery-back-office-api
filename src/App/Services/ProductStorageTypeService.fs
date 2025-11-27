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

