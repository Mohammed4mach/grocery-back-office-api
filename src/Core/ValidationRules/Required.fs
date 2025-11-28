namespace Core.ValidationRules

open System
open Core.Interfaces
open Core.Exceptions.Validation

/// <summary>
/// Ensure that a field is required
/// </summary>
/// <typeparam name="'T">Type of the value</typeparam>
/// <param name="attributeName">
/// The name of the field to include in exception message
/// </param>
/// <param name="value">The value to be validated</param>
type Required<'T> (attributeName : string, value : 'T) =
    interface IValidationRule with
        member _.Validate() : unit =
            let error = RequiredFieldError $"{attributeName} is required"

            try
                if String.IsNullOrEmpty(value.ToString()) then
                    raise error
            with _ -> raise error

