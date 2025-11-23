namespace Core.ValidationRules.Strings

open Core.Interfaces
open Core.Exceptions.Validation

type Max (attributeName : string, value : string, maxVal : int) =
    interface IValidationRule with
        member _.Validate() : unit =
            if value.Length > maxVal then
                raise (MaxLengthExceededError($"Maximum length for {attributeName} is {maxVal}"))

