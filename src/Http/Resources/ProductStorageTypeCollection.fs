namespace Http.Resources

open Core.Entities

type ProductStorageTypeCollection =
    {
        data : ProductStorageTypeResourceData seq
    }

    static member ofEntity (types : ProductStorageType seq) : ProductStorageTypeCollection =
        let data : ProductStorageTypeResourceData seq =
            seq {
                for _type in types do
                    yield ProductStorageTypeResourceData.ofEntity _type
            }

        { data = data }

