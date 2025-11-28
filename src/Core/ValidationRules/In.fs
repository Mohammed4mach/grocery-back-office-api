namespace Core.ValidationRules

open System.Collections.Generic
open Core.Interfaces
open Core.Exceptions.Validation

/// <summary>
/// Ensure that value is included in a given collection
/// </summary>
/// <typeparam name="'T">Type of the value</typeparam>
/// <param name="attributeName">
/// The name of the field to include in exception message
/// </param>
/// <param name="value">The value to be validated</param>
/// <param name="collection">The collection used in validation</param>
type In<'T when 'T : equality> (attributeName : string, value : 'T, collection : 'T seq) =
    interface IValidationRule with
        member _.Validate() : unit =
            try
                collection |> Seq.find (fun (elem : 'T) -> value = elem) |> ignore
            with
                | :? KeyNotFoundException -> raise (BadRequestError $"Invalid {attributeName}")

