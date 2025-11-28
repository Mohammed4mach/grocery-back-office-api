namespace App.Services

open System
open Core.Entities
open Core.Exceptions.Validation
open App.Repositories
open App.Interfaces
open Infrastructure.Core.Types

module OrderService =
    let private repo = OrderRepository :> IRepository<Order | null>

    let index<'Y when 'Y : null> (filters : Condition<'Y> seq) : Order seq =
        let orders = repo.get [] filters

        orders

    let show (id : int) : Order =
        let order = repo.find (id.ToString()) []

        order

    let updateOrderTotalCost (id : int) : Order =
        let items = OrderItemService.index id []

        let totalCost = items |> Seq.fold<OrderItem, float> (fun acc item -> acc + item.cost_per_item * float item.quantity) 0.00

        let order = {
            Order.Default with
                total_cost = totalCost
        }

        repo.partialUpdate (id.ToString()) [ "total_cost" ] order

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

    let update (id : int) (updatedOrder : Order) : Order =
        let order = repo.find (id.ToString())

        repo.update (id.ToString()) updatedOrder

    let delete (id : int) : unit =
        repo.delete (id.ToString())

