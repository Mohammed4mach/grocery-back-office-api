namespace App.Services

open Core.Entities
open Infrastructure.Repositories
open Infrastructure.Core.Types

module OrderItemService =
    let index (filters : Condition seq) : OrderItem seq =
        let items = OrderItemRepository.get [] filters

        items

    let show (id : int) : OrderItem =
        let item = OrderItemRepository.find (id.ToString()) []

        item

    let store (item : OrderItem) : OrderItem =
        OrderItemRepository.store item

    let update (id : int) (updatedItem : OrderItem) : OrderItem =
        let item = OrderItemRepository.find (id.ToString())

        OrderItemRepository.update (id.ToString()) updatedItem

    let delete (id : int) : unit =
        OrderItemRepository.delete (id.ToString())

