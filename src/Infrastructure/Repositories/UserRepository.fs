namespace Infrastructure.Repositories

open Core.Entities

[<AutoOpen>]
module User =
    let UserRepository : Repository<User | null> = {
        Repository.Default with
            table = "users"
            fillable = [
                "fullname"
                "username"
                "password"
                "is_super"
            ]
    }

