namespace Http.Requests

open Core.Interfaces
open Core.ValidationRules

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
                new Required("username", this.username)
                (* password validation *)
                new Required("password", this.password)
            ]

