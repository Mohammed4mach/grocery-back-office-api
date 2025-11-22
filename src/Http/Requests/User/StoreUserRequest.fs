namespace Http.Requests

open Core.Interfaces
open Core.ValidationRules

[<CLIMutable>]
type StoreUserRequest =
    {
        fullname : string
        username : string
        password : string
    }

    interface IValidatable with
        member this.Rules (): IValidationRule seq =
            let rules : IValidationRule seq =
                [
                    (* fullname validation *)
                    (new Strings.Min("fullname", this.fullname, 2)); (new Strings.Max("fullname", this.fullname, 255))
                    (* username validation *)
                    (new Strings.Min("username", this.username, 2)); (new Strings.Max("username", this.username, 255))
                    (new Unique<string>("username", this.username, "users", "username"))
                    (* password validation *)
                    (new Strings.Min("password", this.password, 8)); (new Strings.Max("password", this.password, 255))
                ]

            rules

