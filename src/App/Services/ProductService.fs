namespace App.Services

open Core.Entities
open Infrastructure.Repositories
open Infrastructure.Core.Types

module ProductService =
    let index (filters : Condition seq) : Product seq =
        let products = ProductRepository.get filters

        products

    let show (id : string) : Product =
        let product = ProductRepository.find id

        product

    let store (product : Product) : unit =
        ProductRepository.store product

    let update (id : string) (updatedProduct : Product) : unit =
        let product = ProductRepository.find id

        ProductRepository.update id updatedProduct

    let delete (id : string) : unit =
        ProductRepository.delete id

