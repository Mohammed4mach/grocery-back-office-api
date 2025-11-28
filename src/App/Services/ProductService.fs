namespace App.Services

open Core.Entities
open App.Repositories
open App.Interfaces
open Infrastructure.Core.Types

/// <summary>
/// Module that carry services that handle the business logic regarding
/// product resource
/// </summary>
module ProductService =
    let private repo     = ProductRepository :> IRepository<Product | null>
    let private typeRepo = ProductStorageTypeRepository :> IRepository<ProductStorageType | null>

    /// <summary>
    /// Get a collection of the resource
    /// </summary>
    /// <typeparam name="'Y">Conditions values type</typeparam>
    /// <param name="filters">The conditions for filtering the results</param>
    /// <returns>Collection of the resource</returns>
    let index<'Y when 'Y : null> (filters : Condition<'Y > seq) : Product seq =
        let products = repo.get [] filters

        products

    /// <summary>
    /// Get the product based on the identifier
    /// </summary>
    /// <param name="id">Identifier of the product</param>
    /// <returns>
    /// The product that match for the identifier and the related storage type
    /// </returns>
    let show (id : int) : Product * ProductStorageType =
        let product = repo.find (id.ToString()) []

        // Get storage type
        let storageType = typeRepo.find (product.product_storage_type_id.ToString()) []

        product, storageType

    /// <summary>
    /// Store a product
    /// </summary>
    /// <param name="product">The product to be stored</param>
    /// <returns>The stored product</returns>
    let store (product : Product) : Product =
        repo.store product

    /// <summary>
    /// Update the product that match for the identifier
    /// </summary>
    /// <param name="id">The identifier</param>
    /// <param name="updatedProduct">The values to be updated</param>
    /// <returns>The updated product</returns>
    let update (id : int) (updatedProduct : Product) : Product =
        let product = repo.find (id.ToString())

        repo.update (id.ToString()) updatedProduct

    /// <summary>
    /// Delete the product that match for the identifier
    /// </summary>
    /// <param name="id">The identifier</param>
    let delete (id : int) : unit =
        repo.delete (id.ToString())

