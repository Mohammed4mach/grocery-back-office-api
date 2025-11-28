namespace Core.ValidationRules.Strings

open Core.Interfaces
open Core.Exceptions.Validation

/// <summary>
/// Ensure that a string length is not greater than a given length
/// </summary>
/// <param name="attributeName">
/// The name of the field to include in exception message
/// </param>
/// <param name="value">The string to be validated</param>
/// <param name="maxLength">The maximum length</param>
type Max (attributeName : string, value : string, maxLength : int) =
    interface IValidationRule with
        member _.Validate() : unit =
            if value.Length > maxLength then
                raise (MaxLengthExceededError $"Maximum length for {attributeName} is {maxLength}")

