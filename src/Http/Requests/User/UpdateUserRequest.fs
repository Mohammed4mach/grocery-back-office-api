namespace Http.Requests

open Core.Interfaces
open Core.ValidationRules

[<CLIMutable>]
type UpdateUserRequest =
    {
        mutable id : int
        fullname   : string
        username   : string
        password   : string
        is_super   : bool
    }

    interface IValidatable with
        member this.Rules (): IValidationRule seq =
            [
                (* User exists *)
                new Exists<int>("id", this.id, "users", "id")
                (* fullname validation *)
                new Required("fullname", this.fullname)
                new Strings.Min("fullname", this.fullname, 2); new Strings.Max("fullname", this.fullname, 255)
                (* username validation *)
                new Required("username", this.username)
                new Strings.Min("username", this.username, 2); new Strings.Max("username", this.username, 255)
                new UniqueIgnore<string, int>("username", this.username, "users", "username", "id", this.id)
                (* password validation *)
                new Required("password", this.password)
            ]

