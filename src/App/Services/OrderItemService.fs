namespace App.Services

open Core.Entities
open Core.Exceptions.Validation
open App.Repositories
open App.Interfaces
open Infrastructure.Core.Types

module OrderItemService =
    let private repo        = OrderItemRepository :> IRepository<OrderItem | null>
    let private orderRepo   = OrderRepository :> IRepository<Order | null>
    let private productRepo = ProductRepository :> IRepository<Product | null>

    let index (orderId : int) (filters : Condition<string> seq) : OrderItem seq =
        // Check if order exists
        let order : Order = orderRepo.find (orderId.ToString()) []

        let conditions = (List.ofSeq filters) @ [ Helpers.Database.where "order_id" (Some (order.id.ToString())) ]
        let items      = repo.get [] conditions

        items

    let show (id : int) : OrderItem * Product =
        let item : OrderItem  = repo.find (id.ToString()) []
        let product : Product = productRepo.find (item.product_id.ToString()) []

        item, product

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

    let updateQuantity (id : int) (updatedItem : OrderItem) : OrderItem =
        let item = repo.find (id.ToString())

        repo.partialUpdate (id.ToString()) [ "quantity" ] updatedItem

    let delete (id : int) : unit =
        repo.delete (id.ToString())

