namespace Infrastructure.Repositories

[<AutoOpen>]
module Customer =
    type Customer = {
        id: int
        fullname: string
        address: string
    }

