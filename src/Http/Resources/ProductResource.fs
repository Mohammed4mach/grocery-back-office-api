namespace Http.Resources

open Core.Entities

type ProductResourceData =
    {
        id: int
        name: string
        price: float
        description: string
        product_storage_type_id: int
    }

type ProductResource =
    {
        data : ProductResourceData
    }

    static member ofEntity (product : Product) : ProductResource =
        let data : ProductResourceData = {
            id                      = product.id
            name                    = product.name
            price                   = product.price
            description             = product.description
            product_storage_type_id = product.product_storage_type_id
        }

        let resource : ProductResource = { data = data }

        resource

