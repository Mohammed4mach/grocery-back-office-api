namespace Core.ValidationRules.Numerics

open Core.Interfaces
open Core.Exceptions.Validation

type Max<'T when ^T : comparison>(attributeName : string, value : 'T, maxVal : 'T) =
    interface IValidationRule with
        member _.Validate() : unit =
            if value > maxVal then
                raise (MaxValueExceededError($"Maximum value for {attributeName} is {maxVal.ToString()}"))

