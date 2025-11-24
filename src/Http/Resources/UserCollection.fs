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
                    yield UserResourceData.ofEntity user
            }

        { data = data }

