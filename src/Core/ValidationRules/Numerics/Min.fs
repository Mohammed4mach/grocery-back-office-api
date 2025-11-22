namespace Core.ValidationRules.Numerics

open Core.Interfaces
open Core.Exceptions.Validation

type Min (attributeName : string, value : int, minVal : int) =
    interface IValidationRule with
        member _.Validate() : unit =
            if value < minVal then
                raise (MinValueError($"Min value for {attributeName} is {minVal.ToString()}"))

