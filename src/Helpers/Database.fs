namespace Helpers

open Infrastructure.Core.Types

module Database =
    let makeCondition (column : string) (value : string option) (_operator : string option) : Condition =
        let operator = defaultArg _operator "="
        let condition : Condition = { column = column; operator = operator; value = value }

        condition

    let where (column : string) (value : string option) =
        makeCondition column value (Some "=")

    let whereIn (column : string) (values : string seq) =
        let valueStr = (values |> Seq.fold (fun (acc) (value) -> $"{acc} {value}, ") "").Trim ','

        makeCondition column (Some $"({valueStr})") (Some "IN")

    let whereLike (column : string) (value : string option) =
        makeCondition column value (Some "LIKE")

    let whereGreaterThan (column : string) (value : string option) =
        makeCondition column value (Some ">")

    let whereLessThan (column : string) (value : string option) =
        makeCondition column value (Some "<")

    let whereGreaterOrEqual (column : string) (value : string option) =
        makeCondition column value (Some ">=")

    let whereLessOrEqual (column : string) (value : string option) =
        makeCondition column value (Some "<")

