namespace Http.Requests

open Core.Interfaces
open Core.ValidationRules

/// <summary>
/// Validatable request that validate customer store data
/// </summary>
[<CLIMutable>]
type StoreCustomerRequest =
    {
        fullname : string
        address  : string
    }

    interface IValidatable with
        member this.Rules (): IValidationRule seq =
            [
                (* fullname validation *)
                new Required<string>("fullname", this.fullname)
                new Strings.Min("fullname", this.fullname, 2); new Strings.Max("fullname", this.fullname, 255)
                (* username validation *)
                new Required<string>("address", this.address)
                new Strings.Min("address", this.address, 2); new Strings.Max("address", this.address, 255)
            ]

