namespace Http.Resources

open Core.Entities

type ProductCollection =
    {
        data : ProductResourceData seq
    }

    static member ofEntity (products : Product seq) : ProductCollection =
        let data : ProductResourceData seq =
            seq {
                for product in products do
                    yield ProductResourceData.ofEntity product
            }

        { data = data }

