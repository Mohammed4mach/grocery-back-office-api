namespace Helpers

open Infrastructure.Core.Types

module Database =
    let makeCondition (column : string) (value : string option) (_operator : string option) : Condition =
        let operator = defaultArg _operator "="
        let condition : Condition = { column = column; operator = operator; value = value }

        condition

    let where (column : string) (value : string option) : Condition =
        makeCondition column value (Some "=")

    let whereNot (column : string) (value : string option) : Condition =
        makeCondition column value (Some "<>")

    let whereIn (column : string) (values : string seq) : Condition =
        let valueStr = (values |> Seq.fold (fun (acc) (value) -> $"{acc} {value}, ") "").Trim ','

        makeCondition column (Some $"({valueStr})") (Some "IN")

    let whereLike (column : string) (value : string option) : Condition =
        makeCondition column value (Some "LIKE")

    let whereGreaterThan (column : string) (value : string option) : Condition =
        makeCondition column value (Some ">")

    let whereLessThan (column : string) (value : string option) : Condition =
        makeCondition column value (Some "<")

    let whereGreaterOrEqual (column : string) (value : string option) : Condition =
        makeCondition column value (Some ">=")

    let whereLessOrEqual (column : string) (value : string option) : Condition =
        makeCondition column value (Some "<")

    let makeAggregate (_type : string) (_param : string | null) : AggregateOperation =
        let param =
            match _param with
            | null -> "*"
            | str -> str

        match _type with
        | value when value = "count" -> AggregateOperation.Count param
        | value when value = "avg" -> AggregateOperation.Avg param
        | value when value = "sum" -> AggregateOperation.Sum param
        | value when value = "max" -> AggregateOperation.Max param
        | value when value = "min" -> AggregateOperation.Min param
        | _ -> AggregateOperation.Count param

    let count (_param : string | null) : AggregateOperation =
        makeAggregate "count" _param

    let average (_param : string | null) : AggregateOperation =
        makeAggregate "avg" _param

    let sum (_param : string | null) : AggregateOperation =
        makeAggregate "sum" _param

    let max (_param : string | null) : AggregateOperation =
        makeAggregate "max" _param

    let min (_param : string | null) : AggregateOperation =
        makeAggregate "min" _param

