namespace Http.Resources

open Core.Entities

type CustomerResourceData =
    {
        id: int
        fullname: string
        address: string
    }

type CustomerResource =
    {
        data : CustomerResourceData
    }

    static member ofEntity (customer : Customer) : CustomerResource =
        let data : CustomerResourceData = {
            id       = customer.id
            fullname = customer.fullname
            address  = customer.address
        }

        let resource : CustomerResource = { data = data }

        resource

