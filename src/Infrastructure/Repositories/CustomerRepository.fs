namespace Infrastructure.Repositories

open Core.Entities

[<AutoOpen>]
module Customer =
    let CustomerRepository : Infrastructure.Repositories.Repository<Customer> = {
        Repository.Default with
            table = "customers"
            fillable = [
                "id"
                "fullname"
                "address"
            ]
    }

