namespace Http.Resources

open Core.Entities

type ProductWithRelationsResourceData =
    {
        id: int
        name: string
        price: float
        description: string | null
        product_storage_type: ProductStorageType
    }

    static member ofEntity (product : Product) (storageType : ProductStorageType) : ProductWithRelationsResourceData =
        {
            id                   = product.id
            name                 = product.name
            price                = product.price
            description          = product.description
            product_storage_type = storageType
        }

type ProductWithRelationsResource =
    {
        data : ProductWithRelationsResourceData
    }

    static member ofEntity (product : Product) (storageType : ProductStorageType) : ProductWithRelationsResource =
        { data = ProductWithRelationsResourceData.ofEntity product storageType }

