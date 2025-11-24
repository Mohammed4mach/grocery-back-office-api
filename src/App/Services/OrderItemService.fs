namespace App.Services

open Core.Entities
open Infrastructure.Repositories
open Infrastructure.Core.Types

module OrderItemService =
    let index (filters : Condition seq) : OrderItem seq =
        let items = OrderItemRepository.get filters

        items

    let show (id : string) : OrderItem =
        let item = OrderItemRepository.find id

        item

    let store (item : OrderItem) : OrderItem =
        OrderItemRepository.store item

    let update (id : string) (updatedItem : OrderItem) : OrderItem =
        let item = OrderItemRepository.find id

        OrderItemRepository.update id updatedItem

    let delete (id : string) : unit =
        OrderItemRepository.delete id

