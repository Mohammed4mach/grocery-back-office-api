namespace Helpers

open Infrastructure.Core.Types

/// <summary>
/// Module that holds helpers for database operations
/// </summary>
module Database =

    /// <summary>
    /// Make a condition
    /// </summary>
    /// <typeparam name="'T">Type of the value used in the condition</typeparam>
    /// <param name="column">Column name</param>
    /// <param name="value">Condition value</param>
    /// <param name="_operator">Condition operator</param>
    /// <returns>The database condition</returns>
    let makeCondition<'T> (column : string) (value : 'T option) (_operator : string option) : Condition<'T> =
        let operator = defaultArg _operator "="
        let condition : Condition<'T> = { column = column; operator = operator; value = value }

        condition

    /// <summary>
    /// Make a condition that make use of the `=` operator
    /// </summary>
    /// <typeparam name="'T">Type of the value used in the condition</typeparam>
    /// <param name="column">Column name</param>
    /// <param name="value">Condition value</param>
    /// <returns>The database condition</returns>
    let where<'T> (column : string) (value : 'T option) : Condition<'T> =
        makeCondition column value (Some "=")

    /// <summary>
    /// Make a condition that make use of the `<>` operator
    /// </summary>
    /// <typeparam name="'T">Type of the value used in the condition</typeparam>
    /// <param name="column">Column name</param>
    /// <param name="value">Condition value</param>
    /// <returns>The database condition</returns>
    let whereNot<'T> (column : string) (value : 'T option) : Condition<'T> =
        makeCondition column value (Some "<>")

    /// <summary>
    /// Make a condition that make use of the `IN` operator
    /// </summary>
    /// <typeparam name="'T">Type of the value used in the condition</typeparam>
    /// <param name="column">Column name</param>
    /// <param name="values">Collection of values</param>
    /// <returns>The database condition</returns>
    let whereIn<'T> (column : string) (values : 'T array) : Condition<'T array> =
        makeCondition column (Some values) (Some "IN")

    /// <summary>
    /// Make a condition that make use of the `LIKE` operator
    /// </summary>
    /// <typeparam name="'T">Type of the value used in the condition</typeparam>
    /// <param name="column">Column name</param>
    /// <param name="value">Condition value</param>
    /// <returns>The database condition</returns>
    let whereLike (column : string) (value : 'T option) : Condition<'T> =
        makeCondition column value (Some "LIKE")

    /// <summary>
    /// Make a condition that make use of the `>` operator
    /// </summary>
    /// <typeparam name="'T">Type of the value used in the condition</typeparam>
    /// <param name="column">Column name</param>
    /// <param name="value">Condition value</param>
    /// <returns>The database condition</returns>
    let whereGreaterThan (column : string) (value : 'T option) : Condition<'T> =
        makeCondition column value (Some ">")

    /// <summary>
    /// Make a condition that make use of the `<` operator
    /// </summary>
    /// <typeparam name="'T">Type of the value used in the condition</typeparam>
    /// <param name="column">Column name</param>
    /// <param name="value">Condition value</param>
    /// <returns>The database condition</returns>
    let whereLessThan (column : string) (value : 'T option) : Condition<'T> =
        makeCondition column value (Some "<")

    /// <summary>
    /// Make a condition that make use of the `>=` operator
    /// </summary>
    /// <typeparam name="'T">Type of the value used in the condition</typeparam>
    /// <param name="column">Column name</param>
    /// <param name="value">Condition value</param>
    /// <returns>The database condition</returns>
    let whereGreaterOrEqual (column : string) (value : 'T option) : Condition<'T> =
        makeCondition column value (Some ">=")

    /// <summary>
    /// Make a condition that make use of the `<=` operator
    /// </summary>
    /// <typeparam name="'T">Type of the value used in the condition</typeparam>
    /// <param name="column">Column name</param>
    /// <param name="value">Condition value</param>
    /// <returns>The database condition</returns>
    let whereLessOrEqual (column : string) (value : 'T option) : Condition<'T> =
        makeCondition column value (Some "<=")

    /// <summary>
    /// Make one of aggregate operations types
    /// </summary>
    /// <param name="_type">Type of the aggregation</param>
    /// <param name="_param">Parameter of the aggregate operation</param>
    /// <returns>The aggregate operation type</returns>
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

    /// <summary>
    /// Aggregate operation of type count
    /// </summary>
    /// <param name="_param">Parameter of the aggregate operation</param>
    /// <returns>The count aggregate operation</returns>
    let count (_param : string | null) : AggregateOperation =
        makeAggregate "count" _param

    /// <summary>
    /// Aggregate operation of type average
    /// </summary>
    /// <param name="_param">Parameter of the aggregate operation</param>
    /// <returns>The average aggregate operation</returns>
    let average (_param : string | null) : AggregateOperation =
        makeAggregate "avg" _param

    /// <summary>
    /// Aggregate operation of type sum
    /// </summary>
    /// <param name="_param">Parameter of the aggregate operation</param>
    /// <returns>The sum aggregate operation</returns>
    let sum (_param : string | null) : AggregateOperation =
        makeAggregate "sum" _param

    /// <summary>
    /// Aggregate operation of type max
    /// </summary>
    /// <param name="_param">Parameter of the aggregate operation</param>
    /// <returns>The max aggregate operation</returns>
    let max (_param : string | null) : AggregateOperation =
        makeAggregate "max" _param

    /// <summary>
    /// Aggregate operation of type min
    /// </summary>
    /// <param name="_param">Parameter of the aggregate operation</param>
    /// <returns>The min aggregate operation</returns>
    let min (_param : string | null) : AggregateOperation =
        makeAggregate "min" _param

    /// <summary>
    /// Make a database join
    /// </summary>
    /// <typeparam name="'T">Type of the value of the condition</typeparam>
    /// <param name="_type">Type of the join</param>
    /// <param name="table">The related table to be joined</param>
    /// <param name="condition">The condition on which records are joined</param>
    /// <returns>Database join</returns>
    let private joinAux<'T> (_type : string) (table : string) (condition : Condition<'T>) : Join<'T> =
        {
            _type     = _type
            table     = table
            condition = condition
        }

    /// <summary>
    /// Make a database join
    /// </summary>
    /// <typeparam name="'T">Type of the value of the condition</typeparam>
    /// <param name="_type">Type of the join</param>
    /// <param name="table">The related table to be joined</param>
    /// <param name="condition">The condition on which records are joined</param>
    /// <returns>Database join</returns>
    let join<'T> (table : string) (condition : Condition<'T>) : Join<'T> =
        joinAux "" table condition

    /// <summary>
    /// Make an inner join
    /// </summary>
    /// <typeparam name="'T">Type of the value of the condition</typeparam>
    /// <param name="table">The related table to be joined</param>
    /// <param name="condition">The condition on which records are joined</param>
    /// <returns>Database join</returns>
    let innerJoin<'T> (table : string) (condition : Condition<'T>) : Join<'T> =
        joinAux "INNER" table condition

    /// <summary>
    /// Make a left join
    /// </summary>
    /// <typeparam name="'T">Type of the value of the condition</typeparam>
    /// <param name="table">The related table to be joined</param>
    /// <param name="condition">The condition on which records are joined</param>
    /// <returns>Database join</returns>
    let leftJoin<'T> (table : string) (condition : Condition<'T>) : Join<'T> =
        joinAux "LEFT" table condition

    /// <summary>
    /// Make a right join
    /// </summary>
    /// <typeparam name="'T">Type of the value of the condition</typeparam>
    /// <param name="table">The related table to be joined</param>
    /// <param name="condition">The condition on which records are joined</param>
    /// <returns>Database join</returns>
    let rightJoin<'T> (table : string) (condition : Condition<'T>) : Join<'T> =
        joinAux "RIGHT" table condition

