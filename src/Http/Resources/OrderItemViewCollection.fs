namespace Http.Resources

open Core.Entities

type OrderItemViewCollection =
    {
        data : OrderItemViewResourceData seq
    }

    static member ofEntity (items : OrderItemView seq) : OrderItemViewCollection =
        let data : OrderItemViewResourceData seq =
            seq {
                for item in items do
                    yield OrderItemViewResourceData.ofEntity item
            }

        { data = data }

