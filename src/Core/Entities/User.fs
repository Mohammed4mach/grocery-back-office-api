namespace Core.Entities

/// <summary>
/// Entity that model users resource
/// </summary>
[<CLIMutable>]
type User = {
    id       : int
    fullname : string
    username : string
    password : string
    is_super : bool
}

