namespace Infrastructure.Repositories

open Core.Entities
open Infrastructure.Repositories

[<AutoOpen>]
module User =
    let UserRepository : Repository<User> = {
        Repository.Default with
            table = "users"
            fillable = [
                "fullname"
                "username"
                "password"
            ]
    }

