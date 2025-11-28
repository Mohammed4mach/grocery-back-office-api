namespace App.Repositories

open Core.Entities
open Infrastructure.Repositories

/// <summary>
/// Customer entity repository
/// </summary>
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

