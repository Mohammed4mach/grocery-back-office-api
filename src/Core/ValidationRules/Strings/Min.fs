namespace Core.ValidationRules.Strings

open Core.Interfaces
open Core.Exceptions.Validation

/// <summary>
/// Ensure that a string length is not less than a given length
/// </summary>
/// <param name="attributeName">
/// The name of the field to include in exception message
/// </param>
/// <param name="value">The string to be validated</param>
/// <param name="minLength">The minimum length</param>
type Min (attributeName : string, value : string, minLength : int) =
    interface IValidationRule with
        member _.Validate() : unit =
            if value.Length < minLength then
                raise (MinLengthError $"Minimum length for {attributeName} is {minLength}")

