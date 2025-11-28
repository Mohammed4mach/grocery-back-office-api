namespace Core.ValidationRules

open System
open System.Text.RegularExpressions
open Core.Interfaces
open Core.Exceptions.Validation

/// <summary>
/// Ensure that a value match a given foramt
/// </summary>
/// <typeparam name="'T">Type of the value</typeparam>
/// <param name="attributeName">
/// The name of the field to include in exception message
/// </param>
/// <param name="value">The value to be validated</param>
/// <param name="format">Regex string that specify the right format</param>
type MatchFormat<'T when 'T : null and 'T : equality> (attributeName : string, value : 'T, format : string) =
    interface IValidationRule with
        member _.Validate() : unit =
            let isNull, strVal =
                match value = null with
                | true -> true, ""
                | false -> false, value.ToString()

            let charArr = strVal |> Seq.toArray
            let span    = new ReadOnlySpan<char>(charArr)
            let matched = Regex.IsMatch(span, format)

            if not isNull && not matched then
                raise (UnmatchedFormatError $"{attributeName} must follow the pattern {format}")

