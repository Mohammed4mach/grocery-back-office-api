namespace Core.ValidationRules

open Core.Interfaces
open Core.Exceptions
open Infrastructure.Core
open Infrastructure.Core.Types

/// <summary>
/// Ensure that a record of specific field value is exists in the database
/// </summary>
/// <param name="attributeName">
/// The name of the field to include in exception message
/// </param>
/// <param name="value">Value of the field</param>
/// <param name="table">Table name of the resource</param>
/// <param name="column">Column that indicates the field in the database</param>
type Exists<'T> (attributeName: string, value : 'T, table : string, column : string) =
    interface IValidationRule with
        member _.Validate() : unit =
            let conditions : Condition<string> seq = [ Helpers.Database.where column (Some (value.ToString())) ]
            let records    = Database.operations.selectScalar table (Helpers.Database.count "*") conditions

            if records < 1 then
                raise (EntityNotFoundError $"Entity {table} of {attributeName} = {value} not found")
            ()

