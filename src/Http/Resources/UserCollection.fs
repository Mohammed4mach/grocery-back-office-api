namespace Http.Resources

open Core.Entities

type UserCollection =
    {
        data : UserResourceData seq
    }

    static member ofEntity (users : User seq) : UserCollection =
        let data : UserResourceData seq =
            seq {
                for user in users do
                    yield {
                        id       = user.id
                        fullname = user.fullname
                        username = user.username
                        is_super = user.is_super
                    }
            }

        let resource : UserCollection = { data = data }

        resource

