namespace App.Repositories

open Core.Entities
open Infrastructure.Repositories

[<AutoOpen>]
module Customer =
    let CustomerRepository : Repository<Customer | null> = {
        Repository.Default with
            table = "customers"
            fillable = [
                "fullname"
                "address"
            ]
    }

