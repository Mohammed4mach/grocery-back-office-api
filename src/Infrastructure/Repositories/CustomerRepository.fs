namespace Infrastructure.Repositories

open Core.Entities

[<AutoOpen>]
module Customer =
    let CustomerRepository : Repository<Customer | null> = {
        Repository.Default with
            table = "customers"
            fillable = [
                "id"
                "fullname"
                "address"
            ]
    }

