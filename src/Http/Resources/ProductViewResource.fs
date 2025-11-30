namespace Http.Resources

open Core.Entities

type ProductViewResourceData =
    {
        id: int
        name: string
        price: float
        description: string | null
        product_storage_type_id: int
        product_storage_type_name: string
    }

    static member ofEntity (product : ProductView) : ProductViewResourceData =
        {
            id                        = product.id
            name                      = product.name
            price                     = product.price
            description               = product.description
            product_storage_type_id   = product.product_storage_type_id
            product_storage_type_name = product.product_storage_type_name
        }

type ProductViewResource =
    {
        data : ProductViewResourceData
    }

    static member ofEntity (product : ProductView) : ProductViewResource =
        { data = ProductViewResourceData.ofEntity product }

