namespace Helpers

open Core.Interfaces

module Validation =
    let validate (validatable : IValidatable) : unit =
        let rules : IValidationRule seq = validatable.Rules()

        rules |> Seq.iter (fun rule -> rule.Validate())

