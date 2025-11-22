namespace Http.Resources

open Core.Entities

type UserResourceData =
    {
        id: int
        fullname: string
        username: string
    }

type UserResource =
    {
        data : UserResourceData
    }

    static member ofEntity (user : User) : UserResource =
        let data : UserResourceData = {
            id       = user.id
            fullname = user.fullname
            username = user.username
        }

        let resource : UserResource = { data = data }

        resource

