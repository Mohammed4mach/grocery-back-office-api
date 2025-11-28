namespace Core.ValidationRules

open Core.Interfaces
open Core.Exceptions.Validation
open Infrastructure.Core
open Infrastructure.Core.Types

/// <summary>
/// Ensure that a given field value is unique among the column's values
/// ignoring records of another column's value
/// </summary>
/// <typeparam name="'T">Type of the value of the field</typeparam>
/// <typeparam name="'U">Type of the value used in ignoring</typeparam>
/// <param name="attributeName">
/// The name of the field to include in exception message
/// </param>
/// <param name="value">Value to be validated</param>
/// <param name="table">Table name of the resource</param>
/// <param name="column">
/// Column that indicates the field in the database
/// </param>
/// <param name="ignoreColumn">
/// Column that indicates field used for ignoring criteria
/// </param>
/// <param name="ignoreValue">The value of to be ignored</param>
type UniqueIgnore<'T, 'U> (attributeName : string, value : 'T, table : string, column : string, ignoreColumn : string, ignoreValue : 'U) =
    interface IValidationRule with
        member _.Validate() : unit =
            let conditions : Condition<string> seq = [
                Helpers.Database.where column (Some (value.ToString()))
                Helpers.Database.whereNot ignoreColumn (Some (ignoreValue.ToString()))
            ]

            let records = Database.operations.selectScalar table (Helpers.Database.count "*") conditions

            if records > 0 then
                raise (NotUniqueValidationError $"{attributeName} must be unique")
            ()

