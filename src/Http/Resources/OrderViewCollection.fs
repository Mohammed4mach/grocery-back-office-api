namespace Http.Resources

open Core.Entities

type OrderViewCollection =
    {
        data : OrderViewResourceData seq
    }

    static member ofEntity (orders : OrderView seq) : OrderViewCollection =
        let data : OrderViewResourceData seq =
            seq {
                for order in orders do
                    yield OrderViewResourceData.ofEntity order
            }

        { data = data }

