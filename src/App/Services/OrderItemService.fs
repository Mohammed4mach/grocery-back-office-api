namespace App.Services

open Core.Entities
open Core.Exceptions.Validation
open App.Repositories
open App.Interfaces
open Infrastructure.Core.Types

/// <summary>
/// Module that carry services that handle the business logic regarding
/// order item resource
/// </summary>
module OrderItemService =
    let private repo        = OrderItemRepository :> IRepository<OrderItem | null>
    let private viewRepo    = OrderItemViewRepository :> IRepository<OrderItemView | null>
    let private orderRepo   = OrderRepository :> IRepository<Order | null>
    let private productRepo = ProductRepository :> IRepository<Product | null>

    /// <summary>
    /// Get a collection of the resource related to the order
    /// </summary>
    /// <param name="orderId">The order id</param>
    /// <param name="filters">The conditions for filtering the results</param>
    /// <returns>Collection of the resource</returns>
    let index (orderId : int) (filters : Condition<string> seq) : OrderItem seq =
        // Check if order exists
        let order : Order = orderRepo.find (orderId.ToString()) []

        let conditions = (List.ofSeq filters) @ [ Helpers.Database.where "order_id" (Some (order.id.ToString())) ]
        let items      = repo.get [] conditions

        items

    /// <summary>
    /// Get a collection of the resource related to the order
    /// </summary>
    /// <param name="orderId">The order id</param>
    /// <param name="filters">The conditions for filtering the results</param>
    /// <returns>Collection of the resource</returns>
    let indexView (orderId : int) (filters : Condition<string> seq) : OrderItemView seq =
        // Check if order exists
        let order : Order = orderRepo.find (orderId.ToString()) []

        let conditions = (List.ofSeq filters) @ [ Helpers.Database.where "order_id" (Some (order.id.ToString())) ]
        let items      = viewRepo.get [] conditions

        items

    /// <summary>
    /// Get item based on the identifier
    /// </summary>
    /// <param name="id">Identifier of the item</param>
    /// <returns>The item that match for the identifier and the related product</returns>
    let show (id : int) : OrderItem * Product =
        let item : OrderItem  = repo.find (id.ToString()) []
        let product : Product = productRepo.find (item.product_id.ToString()) []

        item, product

    /// <summary>
    /// Add order item to the order
    /// </summary>
    /// <param name="orderId">The id of the related order</param>
    /// <param name="item">The item to be stored</param>
    /// <param name="escapeUniqueCheck">
    /// Flag to enable/disable check for same product in the order
    /// </param>
    /// <returns>The stored order item</returns>
    let store (orderId : int) (item : OrderItem) (escapeUniqueCheck : bool) : OrderItem =
        if not escapeUniqueCheck then
            // Check for unique (order_id - product_id)
            let conditions    : Condition<string> seq = [
                Helpers.Database.where "order_id" (Some (orderId.ToString()))
                Helpers.Database.where "product_id" (Some (item.product_id.ToString()))
            ]

            let sameItemCount : int = repo.count conditions

            if sameItemCount > 0 then
                raise (ConflictError "Product had already been added to this order")

        let product : Product = productRepo.find (item.product_id.ToString()) []

        let item =
            {
                item with
                    cost_per_item = product.price
                    order_id = orderId
            }

        repo.store item

    /// <summary>
    /// Update the quantity of product of the item that match for the identifier
    /// </summary>
    /// <param name="id">The identifier</param>
    /// <param name="updatedItem">The values to be updated</param>
    /// <returns>The updated order item</returns>
    let updateQuantity (id : int) (updatedItem : OrderItem) : OrderItem =
        let item = repo.find (id.ToString())

        repo.partialUpdate (id.ToString()) [ "quantity" ] updatedItem

    /// <summary>
    /// Delete the item that match for the identifier
    /// </summary>
    /// <param name="id">The identifier</param>
    let delete (id : int) : unit =
        repo.delete (id.ToString())

