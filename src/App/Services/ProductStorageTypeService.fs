namespace App.Services

open Core.Entities
open App.Repositories
open App.Interfaces
open Infrastructure.Core.Types

/// <summary>
/// Module that carry services that handle the business logic regarding
/// product storage type resource
/// </summary>
module ProductStorageTypeService =
    let private repo = ProductStorageTypeRepository :> IRepository<ProductStorageType | null>

    /// <summary>
    /// Get a collection of the resource
    /// </summary>
    /// <typeparam name="'Y">Conditions values type</typeparam>
    /// <param name="filters">The conditions for filtering the results</param>
    /// <returns>Collection of the resource</returns>
    let index<'Y when 'Y : null> (filters : Condition<'Y> seq) : ProductStorageType seq =
        let productTypes = repo.get [] filters

        productTypes

    /// <summary>
    /// Get the record of the resource based on the identifier
    /// </summary>
    /// <param name="id">Identifier of the record</param>
    /// <returns>The entity that match for the identifier</returns>
    let show (id : int) : ProductStorageType =
        let productType = repo.find (id.ToString()) []

        productType

    /// <summary>
    /// Store a record of the resource
    /// </summary>
    /// <param name="productType">The product storage type to be stored</param>
    /// <returns>The stored storage type</returns>
    let store (productType : ProductStorageType) : ProductStorageType =
        repo.store productType

    /// <summary>
    /// Update the record that match for the identifier
    /// </summary>
    /// <param name="id">The identifier</param>
    /// <param name="updatedProductStorageType">The values to be updated</param>
    /// <returns>The updated record</returns>
    let update (id : int) (updatedProductStorageType : ProductStorageType) : ProductStorageType =
        let productType = repo.find (id.ToString())

        repo.update (id.ToString()) updatedProductStorageType

    /// <summary>
    /// Delete the record that match for the identifier
    /// </summary>
    /// <param name="id">The identifier</param>
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


