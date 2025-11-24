namespace App.Services

open Core.Entities
open Infrastructure.Repositories
open Infrastructure.Core.Types

module OrderService =
    let index (filters : Condition seq) : Order seq =
        let orders = OrderRepository.get filters

        orders

    let show (id : string) : Order =
        let order = OrderRepository.find id

        order

    let store (order : Order) : Order =
        OrderRepository.store order

    let update (id : string) (updatedOrder : Order) : Order =
        let order = OrderRepository.find id

        OrderRepository.update id updatedOrder

    let delete (id : string) : unit =
        OrderRepository.delete id

