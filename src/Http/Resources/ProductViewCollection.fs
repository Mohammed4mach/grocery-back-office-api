namespace Http.Resources

open Core.Entities

type ProductViewCollection =
    {
        data : ProductViewResourceData seq
    }

    static member ofEntity (products : ProductView seq) : ProductViewCollection =
        let data : ProductViewResourceData seq =
            seq {
                for product in products do
                    yield ProductViewResourceData.ofEntity product
            }

        { data = data }

