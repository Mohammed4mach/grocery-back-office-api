namespace Core.ValidationRules.Dates

open System
open Core.Interfaces
open Core.Exceptions.Validation

/// <summary>
/// Ensure that a date is not less than a specific date
/// </summary>
/// <param name="attributeName">
/// The name of the field to include in exception message
/// </param>
/// <param name="value">The date to be validated</param>
/// <param name="minVal">The date used as minimum date</param>
type Min (attributeName : string, value : DateOnly, minVal : DateOnly) =
    interface IValidationRule with
        member _.Validate() : unit =
            let minValStr = minVal.ToString("yyyy-MM-dd")

            if value > minVal then
                raise (MinValueError($"Minimum value for {attributeName} is {minValStr}"))

