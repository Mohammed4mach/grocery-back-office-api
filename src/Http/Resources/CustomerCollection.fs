namespace Http.Resources

open Core.Entities

type CustomerCollection =
    {
        data : CustomerResourceData seq
    }

    static member ofEntity (customers : Customer seq) : CustomerCollection =
        let data : CustomerResourceData seq =
            seq {
                for customer in customers do
                    yield CustomerResourceData.ofEntity customer
            }

        { data = data }

