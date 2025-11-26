namespace Core.ValidationRules.Dates

open System
open Core.Interfaces
open Core.Exceptions.Validation

type Min (attributeName : string, value : DateOnly, minVal : DateOnly) =
    interface IValidationRule with
        member _.Validate() : unit =
            let minValStr = minVal.ToString("yyyy-MM-dd")

            if value > minVal then
                raise (MinValueError($"Minimum value for {attributeName} is {minValStr}"))

