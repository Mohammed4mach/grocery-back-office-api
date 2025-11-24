namespace Http.Resources

open Core.Entities

type OrderItemCollection =
    {
        data : OrderItemResourceData seq
    }

    static member ofEntity (items : OrderItem seq) : OrderItemCollection =
        let data : OrderItemResourceData seq =
            seq {
                for item in items do
                    yield OrderItemResourceData.ofEntity item
            }

        { data = data }

