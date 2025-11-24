namespace Core.ValidationRules

open Core.Interfaces
open Core.Exceptions
open Infrastructure.Core

type Exists<'T> (attributeName: string, value : 'T, table : string, column : string) =
    interface IValidationRule with
        member _.Validate() : unit =
            let conditions = [ Helpers.Database.where column (Some (value.ToString())) ]
            let records    = Database.operations.select table conditions

            if records.Length < 1 then
                raise (EntityNotFoundError $"Entity {table} of {attributeName} = {value} not found")
            ()

