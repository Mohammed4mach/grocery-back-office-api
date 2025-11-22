namespace Core.ValidationRules.Numerics

open Core.Interfaces
open Core.Exceptions.Validation

type Max (attributeName : string, value : int, maxVal : int) =
    interface IValidationRule with
        member _.Validate() : unit =
            if value > maxVal then
                raise (MaxValueExceededError($"Max value for {attributeName} is {maxVal.ToString()}"))

