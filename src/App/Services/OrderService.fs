namespace App.Services

open System
open Core.Entities
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

    let update (id : int) (updatedOrder : Order) : Order =
        let order = repo.find (id.ToString())

        repo.update (id.ToString()) updatedOrder

    let delete (id : int) : unit =
        repo.delete (id.ToString())

