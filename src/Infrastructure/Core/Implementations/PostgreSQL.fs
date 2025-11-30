namespace Infrastructure.Core.Implementations

open System
open System.Data
open Npgsql
open Dapper
open Dapper.FSharp
open Infrastructure.Core.Types
open Infrastructure.Core.Exceptions

/// <summary>
/// Module that provide implementations of the abstract interface of
/// the database operations for PostgreSQL DBMS
/// </summary>
module PostgreSQL =

    /// <summary>
    /// Module that holds the type handlers for postgre
    /// </summary>
    module private TypeHandlers =

        /// <summary>
        /// Implement handling the casts `TimeOnly` values
        /// </summary>
        type TimeOnlyHandler() =
            inherit SqlMapper.TypeHandler<TimeOnly>()

            override _.SetValue(param, value) =
                (param :?> NpgsqlParameter).Value <- value

            override _.Parse(value) =
                match value with
                | :? TimeSpan as ts -> TimeOnly(ts.Hours, ts.Minutes, ts.Seconds)
                | :? DateTime as dt -> TimeOnly(dt.Hour, dt.Minute, dt.Second)
                | :? TimeOnly as _to -> _to
                | _ -> failwith $"Unexpected value type for TimeOnly: {value.GetType()}"

        /// <summary>
        /// Implement handling the casts `TimeOnly option` values
        /// </summary>
        type TimeOnlyOptionHandler() =
            inherit SqlMapper.TypeHandler<TimeOnly option>()

            override _.SetValue(param, _option) =
                (param :?> NpgsqlParameter).Value <- _option

            override _.Parse(value) =
                match value with
                | :? TimeSpan as ts -> Some(TimeOnly(ts.Hours, ts.Minutes, ts.Seconds))
                | :? DateTime as dt -> Some(TimeOnly(dt.Hour, dt.Minute, dt.Second))
                | :? TimeOnly as _to -> Some(_to)
                | _ -> failwith $"Unexpected value type for TimeOnly option: {value.GetType()}"

        /// <summary>
        /// Implement handling the casts `DateOnly` values
        /// </summary>
        type DateOnlyHandler() =
            inherit SqlMapper.TypeHandler<DateOnly>()

            override _.SetValue(param, value) =
                (param :?> NpgsqlParameter).Value <- value

            override _.Parse(value) =
                match value with
                | :? DateTime as dt -> DateOnly.FromDateTime dt
                | :? DateOnly as _to -> _to
                | _ -> failwith $"Unexpected value type for DateOnly: {value.GetType()}"
        // End TypeHandlers Module

    /// <summary>
    /// Module contains helpers to facilitate manipulating queries and input
    /// </summary>
    module private Helpers =

        /// <summary>
        /// Get the properly formatted fields names string from field sequence
        /// </summary>
        /// <param name="fields">Names of the fields</param>
        /// <returns>The fields string in the proper format</returns>
        let getFieldsStr (fields : string seq) : string =
            String.Join (",", fields)

        /// <summary>
        /// Get the properly formatted fields values string from field sequence
        /// for insert operation
        /// </summary>
        /// <param name="fields">Names of the fields</param>
        /// <returns>The fields string in the proper format</returns>
        let getInsertValueStr (fields : string seq) : string =
            (fields |> Seq.fold (fun acc field -> $"{acc} @{field},") "").Trim().Trim ','

        /// <summary>
        /// Get the properly formatted fields values string from field sequence
        /// for update operation
        /// </summary>
        /// <param name="fields">Names of the fields</param>
        /// <returns>The fields string in the proper format</returns>
        let getUpdateValueStr (fields : string seq) : string =
            let str     = fields |> Seq.fold (fun acc field -> $"{acc} {field} = @{field},") ""
            let trimmed = str.Trim().Trim ','

            trimmed

        /// <summary>
        /// Convert `Condition<'Y>` to proper format
        /// </summary>
        /// <param name="condition">The condition</param>
        /// <returns>The condition in the proper format</returns>
        let getConditionStr<'Y> (condition : Condition<'Y>) : string =
            let value =
                match condition.value with
                | Some value -> value.ToString()
                | None -> "NULL"

            $"{condition.column}::VARCHAR {condition.operator} {value}::VARCHAR"

        /// <summary>
        /// Convert collection of `Condition<'Y>` to proper format
        /// </summary>
        /// <param name="conditions">Sequence of conditions</param>
        /// <returns>The conditions in the proper format</returns>
        let getConditionsStr<'Y> (conditions : Condition<'Y> seq) : string =
            conditions |> Seq.fold (fun acc condition -> $"{acc} AND {getConditionStr condition}") "TRUE"

        /// <summary>
        /// Convert collection of `Condition<'Y>` to parameterized proper format
        /// </summary>
        /// <typeparam name="'Y">Type of the condition's value</typeparam>
        /// <param name="conditions">Sequence of conditions</param>
        /// <returns>
        /// The conditions in the parameterized proper format with object
        /// that holds the parameters
        /// </returns>
        let getParamConditionStr<'Y when 'Y : null> (conditions : Condition<'Y> seq) : string * obj =
            let columnValue =
                seq {
                    for condition in conditions ->
                        let value =
                            match condition.value with
                            | Some value -> value
                            | None -> null

                        condition.column, value
                } |> Map.ofSeq |> Helpers.DynamicObject.ofMap

            let folder = fun acc condition ->
                let operatorWithRHS : string =
                    match condition.operator.ToLower() = "IN".ToLower() with
                    | true -> $"= ANY(@{condition.column})"
                    | false -> $"{condition.operator} @{condition.column}"

                $"{acc} AND {condition.column}::VARCHAR {operatorWithRHS}"

            let conditionsStr = conditions |> Seq.fold folder "TRUE"

            conditionsStr, columnValue

        /// <summary>
        /// Convert `AggregateOperation` to proper field format
        /// </summary>
        /// <param name="operation">The aggregate operation</param>
        /// <returns>Proper string format that represent the operaiton</returns>
        let getFieldFromAggregateOperation (operation : AggregateOperation) : string =
            match operation with
                | AggregateOperation.Count param -> $"COUNT({param})"
                | AggregateOperation.Avg param -> $"AVG({param})"
                | AggregateOperation.Sum param -> $"SUM({param})"
                | AggregateOperation.Max param -> $"MAX({param})"
                | AggregateOperation.Min param -> $"MIN({param})"

        /// <summary>
        /// Convert `Join` to proper string format
        /// </summary>
        /// <param name="join">The join</param>
        /// <returns>Proper string format that represent the join</returns>
        let getJoinStr<'Z> (join : Join<'Z>) : string =
            let { _type = _type; table = table; condition = condition } = join
            let conditionStr = getConditionStr condition

            $"{_type} JOIN {table} ON {conditionStr}"
        // End Helpers Module

    /// <summary>
    /// Get the connection string from configs variables
    /// </summary>
    /// <returns>Connection string</returns>
    let private getConnectionString () : string =
        let host     = Configs.Database.host
        let port     = Configs.Database.port
        let database = Configs.Database.database
        let username = Configs.Database.username
        let password = Configs.Database.password

        $"Server={host};Port={port};Database={database};User Id={username};Password={password};"

    /// <summary>
    /// Get postgre connection
    /// </summary>
    /// <returns>The database connection object</returns>
    let private getConnection () : IDbConnection =
        new NpgsqlConnection(getConnectionString())

    /// <summary>
    /// Configure the postgre connection
    /// </summary>
    let private configurePostgresDatabase () =
        // Test connection
        let connection = getConnection()

        if connection = null then
            raise (DatabaseConnectionError "Error connecting with PostgreSQL DBMS")

        SqlMapper.AddTypeHandler(TypeHandlers.DateOnlyHandler())
        SqlMapper.AddTypeHandler(TypeHandlers.TimeOnlyHandler())
        SqlMapper.AddTypeHandler(TypeHandlers.TimeOnlyOptionHandler())
        PostgreSQL.OptionTypes.register()

    /// <summary>
    /// Safely use the database connection
    /// </summary>
    /// <param name="useCase">The function that make use of the connection</param>
    /// <returns>Whatever the <paramref name="useCase" /> function returns</returns>
    /// <example>
    /// <code>
    ///     useConnection (
    ///         fun connection ->
    ///             let sql = $"DELETE FROM {table} WHERE id = 2"
    ///             connection.Execute (CommandDefinition sql)
    ///     )
    /// </code>
    /// </example>
    let private useConnection (useCase : IDbConnection -> 'T) : 'T =
        let dsBuilder  = new NpgsqlDataSourceBuilder(getConnectionString())
        let datasource = dsBuilder.Build()
        let connection = datasource.OpenConnection()

        if connection = null then
            raise (DatabaseConnectionError "Error connecting with PostgreSQL DBMS")

        try
            try
                connection |> useCase
            with __ -> reraise()
        finally
            connection.Close()
            connection.Dispose()

    /// <summary>
    /// Execute a statement
    /// </summary>
    /// <param name="_param">The execute operation parameter</param>
    /// <returns>The number of affected rows</returns>
    let private execute (_param : ExecuteParameter) : int =
        useConnection (
            fun connection ->
                let (sql : string, paramWrapper : obj option) =
                    match _param with
                    | ExecuteParameter.QueryOnly sql -> sql, Some { new Object() with member _.ToString() = "" }
                    | ExecuteParameter.WithParam (sql, param) -> sql, Some param

                connection.Execute (CommandDefinition (sql, paramWrapper.Value))
        )

    /// <summary>
    /// Insert entity to a table
    /// </summary>
    /// <typeparam name="'T">Type of the entity</typeparam>
    /// <param name="table">Name of the table</param>
    /// <param name="fields">Names of the fields to be inserted</param>
    /// <param name="value">The entity to be inserted</param>
    /// <returns>The inserted entity</returns>
    let private insert<'T> (table : string) (fields : string seq) (value : 'T) : 'T =
        useConnection (
            fun connection ->
                let fieldsStr     = Helpers.getFieldsStr fields
                let valueStr      = Helpers.getInsertValueStr fields

                let sql  = $"INSERT INTO {table} ({fieldsStr}) VALUES ({valueStr}) RETURNING *"

                connection.QuerySingle<'T> (CommandDefinition (sql, value))
        )

    /// <summary>
    /// Update a record of a table
    /// </summary>
    /// <typeparam name="'T">Type of the entity</typeparam>
    /// <typeparam name="'Y">Type of the condition's value</typeparam>
    /// <param name="table">Name of the table</param>
    /// <param name="fields">Names of the fields to be updated</param>
    /// <param name="value">Updated values</param>
    /// <returns>The updated entity</returns>
    let private update<'T, 'P> (table : string) (fields : string seq) (value : 'T) (conditions : Condition<'P> seq) : 'T =
        useConnection (
            fun connection ->
                let valueStr      = Helpers.getUpdateValueStr fields
                let conditionsStr = Helpers.getConditionsStr conditions

                let sql  = $"UPDATE {table} SET {valueStr} WHERE {conditionsStr} RETURNING *"

                connection.QuerySingle<'T> (CommandDefinition (sql, value :> obj))
        )

    /// <summary>
    /// Delete records based on criteria
    /// </summary>
    /// <typeparam name="'Y">Type of the condition's value</typeparam>
    /// <param name="table">Name of the table</param>
    /// <param name="conditions">Criteria of deleting</param>
    /// <param name="value">Updated values</param>
    /// <returns>Number of affected rows</returns>
    let private delete<'T, 'P> (table : string) (conditions : Condition<'P> seq) : int =
        useConnection (
            fun connection ->
                let conditionsStr = Helpers.getConditionsStr conditions

                let sql  = $"DELETE FROM {table} WHERE {conditionsStr}"

                connection.Execute (CommandDefinition sql)
        )

    /// <summary>
    /// Make common logic for processing select parameters
    /// </summary>
    /// <typeparam name="'Y">Type of the condition's value</typeparam>
    /// <typeparam name="'Z">Type of the condition's value of the join</typeparam>
    /// <param name="table">Name of the table</param>
    /// <param name="joins">Ather table joins</param>
    /// <param name="conditions">Criteria to filter the result</param>
    /// <returns>Pair of the query and its parameters</returns>
    let private prepareSelectParams<'Y, 'Z when 'Y : null and 'Z : null> (table : string) (joins : Join<'Z> seq) (conditions : Condition<'Y> seq) : string * obj =
        let conditionsStr, columnValue = Helpers.getParamConditionStr conditions
        let joinStr = Seq.fold (fun (acc : string) (join : Join<'Z>) -> $"{acc} {Helpers.getJoinStr join}") "" joins

        let sql  = $"SELECT * FROM {table} {joinStr} WHERE {conditionsStr}"

        sql, columnValue

    /// <summary>
    /// Get a collection of records from table
    /// </summary>
    /// <typeparam name="'T">Type of the entity</typeparam>
    /// <typeparam name="'Y">Type of the condition's value</typeparam>
    /// <typeparam name="'Z">Type of the condition's value of the join</typeparam>
    /// <param name="table">Name of the table</param>
    /// <param name="joins">Ather table joins</param>
    /// <param name="conditions">Criteria to filter the result</param>
    /// <returns>Collection of records of type `'T`</returns>
    let private select<'T, 'Y, 'Z when 'Y : null and 'Z : null> (table : string) (joins : Join<'Z> seq) (conditions : Condition<'Y> seq) : 'T list =
        useConnection (
            fun connection ->
                let sql, columnValue = prepareSelectParams table joins conditions
                let objStr = sprintf "%A" columnValue

                try
                    connection.Query<'T> (sql, columnValue) |> List.ofSeq
                with ex -> raise (new Exception $"{sql} __ {ex.Message} __ {objStr}")
        )

    /// <summary>
    /// Select a collection from two related tables through joins
    /// </summary>
    /// <typeparam name="'T">Type of the entity</typeparam>
    /// <typeparam name="'U">
    /// Type for entity related to `'T`, this used in `selectWithRelation`
    /// </typeparam>
    /// <typeparam name="'P">
    /// The type of the result of the `selectWithRelation` funciton
    /// </typeparam>
    /// <typeparam name="'Y">Type of the condition's value</typeparam>
    /// <typeparam name="'Z">Type of the condition's value of the join</typeparam>
    /// <param name="table">Name of the table</param>
    /// <param name="joins">Another table joins</param>
    /// <param name="conditions">Criteria to filter the result</param>
    /// <param name="splitOn">
    /// Split on function which form the final result of the operation
    /// </param>
    /// <returns>Collection of records of type `'P`</returns>
    let private selectWithRelation<'T, 'U, 'P,'Y, 'Z when 'Y : null and 'Z : null> (table : string) (joins : Join<'Z> seq) (conditions : Condition<'Y> seq) (splitOn : Func<'T, 'U, 'P>) : 'P list =
        useConnection (
            fun connection ->
                let sql, columnValue = prepareSelectParams table joins conditions

                connection.Query<'T, 'U, 'P> (sql, splitOn, columnValue) |> List.ofSeq
        )

    /// <summary>
    /// Get a record from table
    /// </summary>
    /// <typeparam name="'T">Type of the entity</typeparam>
    /// <typeparam name="'Y">Type of the condition's value</typeparam>
    /// <typeparam name="'Z">Type of the condition's value of the join</typeparam>
    /// <param name="table">Name of the table</param>
    /// <param name="joins">Another table joins</param>
    /// <param name="conditions">Criteria to filter the result</param>
    /// <returns>A record of type `'T`</returns>
    let private selectSingle<'T, 'Y, 'Z when 'Y : null and 'Z : null> (table : string) (joins : Join<'Z> seq) (conditions : Condition<'Y> seq) : 'T =
        useConnection (
            fun connection ->
                let sql, columnValue = prepareSelectParams table joins conditions

                connection.QuerySingleOrDefault<'T> (sql, columnValue)
        )

    /// <summary>
    /// Select a single value from the database
    /// </summary>
    /// <typeparam name="'Y">Type of the condition's value</typeparam>
    /// <typeparam name="'U">
    /// Type of the scalar value retuned by an aggregate.
    /// </typeparam>
    /// <param name="table">Name of the table</param>
    /// <param name="operation">The aggregate operation</param>
    /// <param name="conditions">Criteria to filter the result</param>
    /// <returns>A single value of type `'U`</returns>
    let private selectScalar<'U, 'Y when 'Y : null> (table : string) (operation : AggregateOperation) (conditions : Condition<'Y> seq) : 'U =
        useConnection (
            fun connection ->
                let conditionsStr, columnValue = Helpers.getParamConditionStr conditions
                let field = Helpers.getFieldFromAggregateOperation operation

                let sql  = $"SELECT {field} FROM {table} WHERE {conditionsStr}"

                connection.ExecuteScalar<'U> (sql, columnValue)
        )

    /// <summary>
    /// Implementation of the abstract database interface for postgre DBMS
    /// </summary>
    let operations : Operations<'T, 'U, 'P, 'Y, 'Z> =
        {
            insert             = insert
            update             = update
            delete             = delete
            select             = select
            execute            = execute
            selectSingle       = selectSingle
            selectScalar       = selectScalar
            selectWithRelation = selectWithRelation
            configureDatabase  = configurePostgresDatabase
        }

