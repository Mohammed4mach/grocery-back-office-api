namespace Http.Requests

open Core.Interfaces
open Core.ValidationRules

/// <summary>
/// Validatable request that validate username and password for login
/// </summary>
[<CLIMutable>]
type LoginRequest =
    {
        username : string
        password : string
    }

    interface IValidatable with
        member this.Rules (): IValidationRule seq =
            [
                (* username validation *)
                new Required<string>("username", this.username)
                (* password validation *)
                new Required<string>("password", this.password)
            ]

