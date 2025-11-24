namespace Core.ValidationRules

open System
open System.Collections.Generic
open Core.Interfaces
open Core.Exceptions.Validation

type Required (attributeName : string, value : string) =
    interface IValidationRule with
        member _.Validate() : unit =
            if String.IsNullOrEmpty (value) then
                raise (RequiredFieldError $"{attributeName} is required")

