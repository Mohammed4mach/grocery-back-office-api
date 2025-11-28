namespace Http.Requests

open Core.Interfaces
open Core.ValidationRules

/// <summary>
/// Validatable request that validate customer update data
/// </summary>
[<CLIMutable>]
type UpdateCustomerRequest =
    {
        mutable id : int
        fullname   : string
        address    : string
    }

    interface IValidatable with
        member this.Rules (): IValidationRule seq =
            [
                (* Customer exists *)
                new Exists<int>("id", this.id, "customers", "id")
                (* fullname validation *)
                new Required<string>("fullname", this.fullname)
                new Strings.Min("fullname", this.fullname, 2); new Strings.Max("fullname", this.fullname, 255)
                (* address validation *)
                new Required<string>("address", this.address)
                new Strings.Min("address", this.address, 2); new Strings.Max("address", this.address, 255)
            ]

