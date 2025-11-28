namespace Helpers

open Core.Interfaces

/// <summary>
/// Validation specific helpers
/// </summary>
module Validation =

    /// <summary>
    /// Validate a request that comply to `IValidatable` contract
    /// </summary>
    /// <param name="validatable">The validatable objec</param>
    let validate (validatable : IValidatable) : unit =
        let rules : IValidationRule seq = validatable.Rules()

        rules |> Seq.iter (fun rule -> rule.Validate())

