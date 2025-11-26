namespace Core.ValidationRules.Dates

open System
open Core.Interfaces
open Core.Exceptions.Validation

type Max (attributeName : string, value : DateOnly, maxVal : DateOnly) =
    interface IValidationRule with
        member _.Validate() : unit =
            let maxValStr = maxVal.ToString("yyyy-MM-dd")

            if value > maxVal then
                raise (MaxValueExceededError($"Maximum value for {attributeName} is {maxValStr}"))

