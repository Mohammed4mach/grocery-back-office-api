namespace Core.ValidationRules.Numerics

open Core.Interfaces
open Core.Exceptions.Validation

/// <summary>
/// Ensure that a value is not greater than a given value
/// </summary>
/// <typeparam name="'T">Type of the value to be validated</typeparam>
/// <param name="attributeName">
/// The name of the field to include in exception message
/// </param>
/// <param name="value">The value to be validated</param>
/// <param name="maxVal">The maximum value</param>
type Max<'T when ^T : comparison>(attributeName : string, value : 'T, maxVal : 'T) =
    interface IValidationRule with
        member _.Validate() : unit =
            if value > maxVal then
                raise (MaxValueExceededError($"Maximum value for {attributeName} is {maxVal.ToString()}"))

