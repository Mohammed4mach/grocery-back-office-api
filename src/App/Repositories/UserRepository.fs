namespace App.Repositories

open Core.Entities
open Infrastructure.Repositories

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

