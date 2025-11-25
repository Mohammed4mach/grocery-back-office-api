namespace Core.ValidationRules

open System
open Core.Interfaces
open Core.Exceptions.Validation

type Required<'T> (attributeName : string, value : 'T) =
    interface IValidationRule with
        member _.Validate() : unit =
            let error = RequiredFieldError $"{attributeName} is required"

            try
                if String.IsNullOrEmpty(value.ToString()) then
                    raise error
            with _ -> raise error

