namespace App.Services

open System
open Core.Entities
open Core.Exceptions.Validation
open App.Repositories
open App.Interfaces
open Infrastructure.Core.Types

/// <summary>
/// Module that carry services that handle the business logic regarding
/// order resource
/// </summary>
module OrderService =
    let private repo     = OrderRepository :> IRepository<Order | null>
    let private viewRepo = OrderViewRepository :> IRepository<OrderView | null>

    /// <summary>
    /// Get a collection of the resource
    /// </summary>
    /// <typeparam name="'Y">Conditions values type</typeparam>
    /// <param name="filters">The conditions for filtering the results</param>
    /// <returns>Collection of the resource</returns>
    let index<'Y when 'Y : null> (filters : Condition<'Y> seq) : OrderView seq =
        let orders = viewRepo.get [] filters

        orders

    /// <summary>
    /// Get the order based on the identifier
    /// </summary>
    /// <param name="id">Identifier of the order</param>
    /// <returns>The entity that match for the identifier</returns>
    let show (id : int) : Order =
        let order = repo.find (id.ToString()) []

        order

    /// <summary>
    /// Get the order based on the identifier
    /// </summary>
    /// <param name="id">Identifier of the order</param>
    /// <returns>The entity that match for the identifier</returns>
    let showView (id : int) : OrderView =
        let order = viewRepo.find (id.ToString()) []

        order

    /// <summary>
    /// Update order total cost according to the products and the quantities
    /// </summary>
    /// <param name="id">The order identifier</param>
    /// <param name="deliveryTime">The date and time of the delivery</param>
    /// <returns>The updated order</returns>
    let updateOrderTotalCost (id : int) : Order =
        let items = OrderItemService.index id []

        let totalCost = items |> Seq.fold<OrderItem, float> (fun acc item -> acc + item.cost_per_item * float item.quantity) 0.00

        let order = {
            Order.Default with
                total_cost = totalCost
        }

        repo.partialUpdate (id.ToString()) [ "total_cost" ] order

    /// <summary>
    /// Store order
    /// </summary>
    /// <param name="order">The order to be stored</param>
    /// <param name="items">Sequence of items of the order</param>
    /// <returns>The stored order</returns>
    let store (order : Order) (items : OrderItem seq) : Order =
        let order : Order =
            {
                order with
                    order_time = DateTime.Now
            }

        let order = repo.store order

        items |> Seq.iter<OrderItem> (fun item -> OrderItemService.store order.id item true |> ignore)

        let updatedOrder = order.id |> updateOrderTotalCost

        updatedOrder

    /// <summary>
    /// Set the delivery date and time for the order
    /// </summary>
    /// <param name="id">The order identifier</param>
    /// <param name="deliveryTime">The date and time of the delivery</param>
    /// <returns>The updated order</returns>
    let setDeliveryTime (id : int) (deliveryTime : DeliveryTime) : Order =
        let order : Order = repo.find (id.ToString()) []
        let { date = date; time = time } = deliveryTime

        // Check if the time is valid
        let valid : bool =  deliveryTime |> DeliveryTimeService.isValidDeliveryTime order

        if not valid then
            let dateStr = date.ToString "yyyy-MM-dd"
            let timeStr = time.ToString "HH:mm"

            raise (ConflictError $"The time {timeStr} on {date.DayOfWeek.ToString()} ({dateStr}) is not suitable to deliver this order")

        let isGreen     : bool  = time |> DeliveryTimeService.isGreenTime
        let orderBody   : Order = {
            Order.Default with
                delivery_date     = Nullable(date)
                delivery_time     = Nullable(time)
                is_green_delivery = isGreen
        }

        let fields       : string seq = [ "delivery_date"; "delivery_time"; "is_green_delivery" ]
        let updatedOrder : Order      = orderBody |> repo.partialUpdate (order.id.ToString()) fields

        updatedOrder

    /// <summary>
    /// Update the order that match for the identifier
    /// </summary>
    /// <param name="id">The identifier</param>
    /// <param name="updatedOrder">The values to be updated</param>
    /// <returns>The updated order</returns>
    let update (id : int) (updatedOrder : Order) : Order =
        let order = repo.find (id.ToString())

        repo.update (id.ToString()) updatedOrder

    /// <summary>
    /// Delete the order that match for the identifier
    /// </summary>
    /// <param name="id">The identifier</param>
    let delete (id : int) : unit =
        repo.delete (id.ToString())

