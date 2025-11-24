namespace Core.ValidationRules

open Core.Interfaces
open Core.Exceptions.Validation
open Infrastructure.Core

type UniqueIgnore<'T, 'U> (attributeName : string, value : 'T, table : string, column : string, ignoreColumn : string, ignoreValue : 'U) =
    interface IValidationRule with
        member _.Validate() : unit =
            let conditions = [
                Helpers.Database.where column (Some (value.ToString()))
                Helpers.Database.whereNot ignoreColumn (Some (ignoreValue.ToString()))
            ]

            let records = Database.operations.select table conditions

            if records.Length > 0 then
                raise (NotUniqueValidationError($"{attributeName} must be unique"))
            ()

