namespace Http.Requests

open Core.Interfaces
open Core.ValidationRules

/// <summary>
/// Validatable request that validate password update fields
/// </summary>
[<CLIMutable>]
type UpdatePasswordRequest =
    {
        password : string
        new_password : string
    }

    interface IValidatable with
        member this.Rules (): IValidationRule seq =
            [
                (* password validation *)
                new Required<string>("password", this.password)
                (* password validation *)
                new Required<string>("new_password", this.new_password)
                new Strings.Min("new_password", this.new_password, 8); new Strings.Max("new_password", this.new_password, 255)
            ]

