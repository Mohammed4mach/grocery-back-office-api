namespace Http.Resources

open Core.Entities

type ProductResourceData =
    {
        id: int
        name: string
        price: float
        description: string | null
        product_storage_type_id: int
    }

    static member ofEntity (product : Product) : ProductResourceData =
        {
            id                      = product.id
            name                    = product.name
            price                   = product.price
            description             = product.description
            product_storage_type_id = product.product_storage_type_id
        }

type ProductResource =
    {
        data : ProductResourceData
    }

    static member ofEntity (product : Product) : ProductResource =
        { data = ProductResourceData.ofEntity product }

