namespace Http.Resources

open Core.Entities

type UserResourceData =
    {
        id       : int
        fullname : string
        username : string
        is_super : bool
    }

    static member ofEntity (user : User) : UserResourceData =
        {
            id       = user.id
            fullname = user.fullname
            username = user.username
            is_super = user.is_super
        }

type UserResource =
    {
        data : UserResourceData
    }

    static member ofEntity (user : User) : UserResource =
        { data = UserResourceData.ofEntity user }

