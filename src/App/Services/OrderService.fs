namespace App.Services

open Core.Entities
open Infrastructure.Repositories
open Infrastructure.Core.Types

module OrderService =
    let index (filters : Condition seq) : Order seq =
        let orders = OrderRepository.get filters

        orders

    let show (id : int) : Order =
        let order = OrderRepository.find (id.ToString())

        order

    let store (order : Order) : Order =
        OrderRepository.store order

    let update (id : int) (updatedOrder : Order) : Order =
        let order = OrderRepository.find (id.ToString())

        OrderRepository.update (id.ToString()) updatedOrder

    let delete (id : int) : unit =
        OrderRepository.delete (id.ToString())

