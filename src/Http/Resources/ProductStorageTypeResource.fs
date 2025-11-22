namespace Http.Resources

open Core.Entities

type ProductStorageTypeResourceData =
    {
        id: int
        name: string
        delivery_time_rule_id: string
    }

type ProductStorageTypeResource =
    {
        data : ProductStorageTypeResourceData
    }

    static member ofEntity (_type : ProductStorageType) : ProductStorageTypeResource =
        let data : ProductStorageTypeResourceData = {
            id                    = _type.id
            name                  = _type.name
            delivery_time_rule_id = _type.delivery_time_rule_id
        }

        let resource : ProductStorageTypeResource = { data = data }

        resource

