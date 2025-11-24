namespace Core.ValidationRules.Numerics

open Core.Interfaces
open Core.Exceptions.Validation

type Min<'T when ^T : comparison>(attributeName : string, value : 'T, minVal : 'T) =
    interface IValidationRule with
        member _.Validate() : unit =
            if value < minVal then
                raise (MinValueError($"Minimum value for {attributeName} is {minVal.ToString()}"))

