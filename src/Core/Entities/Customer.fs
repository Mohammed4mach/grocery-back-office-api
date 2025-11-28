namespace Core.Entities

/// <summary>
/// Entity that model the customers resource
/// </summary>
[<CLIMutable>]
type Customer = {
    id: int
    fullname: string
    address: string
}

