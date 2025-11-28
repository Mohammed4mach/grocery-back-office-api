namespace Core.ValidationRules

open Core.Interfaces
open Core.Exceptions.Validation
open Infrastructure.Core
open Infrastructure.Core.Types

/// <summary>
/// Ensure that a given field value is unique among the column's values
/// </summary>
/// <typeparam name="'T">Type of the value</typeparam>
/// <param name="attributeName">
/// The name of the field to include in exception message
/// </param>
/// <param name="value">Value to be validated</param>
/// <param name="table">Table name of the resource</param>
/// <param name="column">
/// Column that indicates the field in the database
/// </param>
type Unique<'T> (attributeName : string, value : 'T, table : string, column : string) =
    interface IValidationRule with
        member _.Validate() : unit =
            let conditions : Condition<string> seq = [ Helpers.Database.where column (Some (value.ToString())) ]
            let records    = Database.operations.selectScalar table (Helpers.Database.count "*") conditions

            if records > 0 then
                raise (NotUniqueValidationError $"{attributeName} must be unique")
            ()

