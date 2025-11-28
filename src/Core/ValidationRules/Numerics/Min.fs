namespace Core.ValidationRules.Numerics

open Core.Interfaces
open Core.Exceptions.Validation

/// <summary>
/// Ensure that a value is not less than a given value
/// </summary>
/// <typeparam name="'T">Type of the value to be validated</typeparam>
/// <param name="attributeName">
/// The name of the field to include in exception message
/// </param>
/// <param name="value">The value to be validated</param>
/// <param name="minVal">The minimum value</param>
type Min<'T when 'T : comparison>(attributeName : string, value : 'T, minVal : 'T) =
    interface IValidationRule with
        member _.Validate() : unit =
            if value < minVal then
                raise (MinValueError($"Minimum value for {attributeName} is {minVal.ToString()}"))

