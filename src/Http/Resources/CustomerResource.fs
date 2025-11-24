namespace Http.Resources

open Core.Entities

type CustomerResourceData =
    {
        id: int
        fullname: string
        address: string
    }

    static member ofEntity (customer : Customer) : CustomerResourceData =
        {
            id       = customer.id
            fullname = customer.fullname
            address  = customer.address
        }


type CustomerResource =
    {
        data : CustomerResourceData
    }

    static member ofEntity (customer : Customer) : CustomerResource =
        { data = CustomerResourceData.ofEntity customer }

