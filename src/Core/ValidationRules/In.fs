namespace Core.ValidationRules

open System.Collections.Generic
open Core.Interfaces
open Core.Exceptions.Validation

type In<'T when 'T : equality> (attributeName : string, value : 'T, collection : 'T seq) =
    interface IValidationRule with
        member _.Validate() : unit =
            try
                collection |> Seq.find (fun (elem : 'T) -> value = elem) |> ignore
            with
                | :? KeyNotFoundException -> raise (BadRequestError $"Invalid {attributeName}")

