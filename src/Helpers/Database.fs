namespace Helpers

open Infrastructure.Core.Types

module Database =
    let makeCondition<'T> (column : string) (value : 'T option) (_operator : string option) : Condition<'T> =
        let operator = defaultArg _operator "="
        let condition : Condition<'T> = { column = column; operator = operator; value = value }

        condition

    let where<'T> (column : string) (value : 'T option) : Condition<'T> =
        makeCondition column value (Some "=")

    let whereNot<'T> (column : string) (value : 'T option) : Condition<'T> =
        makeCondition column value (Some "<>")

    let whereIn<'T> (column : string) (values : 'T seq) : Condition<'T seq> =
        makeCondition column (Some values) (Some "IN")

    let whereLike (column : string) (value : 'T option) : Condition<'T> =
        makeCondition column value (Some "LIKE")

    let whereGreaterThan (column : string) (value : 'T option) : Condition<'T> =
        makeCondition column value (Some ">")

    let whereLessThan (column : string) (value : 'T option) : Condition<'T> =
        makeCondition column value (Some "<")

    let whereGreaterOrEqual (column : string) (value : 'T option) : Condition<'T> =
        makeCondition column value (Some ">=")

    let whereLessOrEqual (column : string) (value : 'T option) : Condition<'T> =
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

    let private joinAux<'T> (_type : string) (table : string) (condition : Condition<'T>) : Join<'T> =
        {
            _type     = _type
            table     = table
            condition = condition
        }

    let join<'T> (table : string) (condition : Condition<'T>) : Join<'T> =
        joinAux "" table condition

    let innerJoin<'T> (table : string) (condition : Condition<'T>) : Join<'T> =
        joinAux "INNER" table condition

    let leftJoin<'T> (table : string) (condition : Condition<'T>) : Join<'T> =
        joinAux "LEFT" table condition

    let rightJoin<'T> (table : string) (condition : Condition<'T>) : Join<'T> =
        joinAux "RIGHT" table condition

