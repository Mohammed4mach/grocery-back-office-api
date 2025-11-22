namespace Core.ValidationRules

open Core.Interfaces
open Core.Exceptions.Validation
open Infrastructure.Core

type Unique<'T> (attributeName : string, value : 'T, table : string, column : string) =
    interface IValidationRule with
        member _.Validate() : unit =
            let conditions = [ Helpers.Database.where column (Some (value.ToString())) ]
            let records    = Database.operations.select table conditions

            if records.Length > 0 then
                raise (NotUniqueValidationError($"{attributeName} must be unique"))
            ()

