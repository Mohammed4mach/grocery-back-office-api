namespace Core.ValidationRules.Dates

open System
open Core.Interfaces
open Core.Exceptions.Validation

/// <summary>
/// Ensure that a date is not greater than a specific date
/// </summary>
/// <param name="attributeName">
/// The name of the field to include in exception message
/// </param>
/// <param name="value">The date to be validated</param>
/// <param name="minVal">The date used as maximum date</param>
type Max (attributeName : string, value : DateOnly, maxVal : DateOnly) =
    interface IValidationRule with
        member _.Validate() : unit =
            let maxValStr = maxVal.ToString("yyyy-MM-dd")

            if value > maxVal then
                raise (MaxValueExceededError($"Maximum value for {attributeName} is {maxValStr}"))

