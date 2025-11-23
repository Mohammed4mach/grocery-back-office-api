namespace Core.ValidationRules.Strings

open Core.Interfaces
open Core.Exceptions.Validation

type Min (attributeName : string, value : string, minVal : int) =
    interface IValidationRule with
        member _.Validate() : unit =
            if value.Length < minVal then
                raise (MinLengthError($"Minimum length for {attributeName} is {minVal}"))

