namespace Http.Resources

open Core.Entities

type OrderCollection =
    {
        data : OrderResourceData seq
    }

    static member ofEntity (orders : Order seq) : OrderCollection =
        let data : OrderResourceData seq =
            seq {
                for order in orders do
                    yield OrderResourceData.ofEntity order
            }

        { data = data }

