namespace Infrastructure.Core

open System

/// <summary>
/// Module contains types used in the abstract database interface used in
/// the application
/// </summary>
module Types =

    /// <summary>
    /// Execute operation parameter
    /// </summary>
    type ExecuteParameter =
        /// <summary>String of the query without parameters</summary>
        | QueryOnly of string
        /// <summary>Pair contains the query and its parameters</summary>
        | WithParam of string * obj

    /// <summary>
    /// Condition type for abstracting database interface. Indicates
    /// query condition
    /// </summary>
    type Condition<'T> =
        {
            column : string
            operator : string
            value : 'T option
        }

    /// <summary>
    /// Join type for abstracting database interface
    /// </summary>
    type Join<'T>=
        {
            _type : string
            table : string
            condition : Condition<'T>
        }

    /// <summary>
    /// Aggregation operation type for abstracting database interface
    /// </summary>
    type AggregateOperation =
        /// <summary>Indicates count operation</summary>
        | Count of string
        /// <summary>Indicates sum operation</summary>
        | Sum of string
        /// <summary>Indicates average operation</summary>
        | Avg of string
        /// <summary>Indicates min operation</summary>
        | Min of string
        /// <summary>Indicates max operation</summary>
        | Max of string

    /// <summary>
    /// Type that holds abstract database operations interface
    /// </summary>
    /// <typeparam name="'T">Type of the entity</typeparam>
    /// <typeparam name="'U">
    /// Type of the scalar value retuned by an aggregate. Also serve as
    /// type for entity related to `'T`, this used in `selectWithRelation` LOL!
    /// </typeparam>
    /// <typeparam name="'P">
    /// The type of the result of the `selectWithRelation` funciton
    /// </typeparam>
    /// <typeparam name="'Y">Type of the condition's value</typeparam>
    /// <typeparam name="'Z">Type of the condition's value of the join</typeparam>
    type Operations<'T, 'U, 'P, 'Y, 'Z> =
        {
            /// <summary>
            /// Insert entity to a table
            /// </summary>
            /// <typeparam name="'T">Type of the entity</typeparam>
            /// <param name="table">Name of the table</param>
            /// <param name="fields">Names of the fields to be inserted</param>
            /// <param name="value">The entity to be inserted</param>
            /// <returns>The inserted entity</returns>
            insert : string -> string seq -> 'T -> 'T

            /// <summary>
            /// Update a record of a table
            /// </summary>
            /// <typeparam name="'T">Type of the entity</typeparam>
            /// <typeparam name="'Y">Type of the condition's value</typeparam>
            /// <param name="table">Name of the table</param>
            /// <param name="fields">Names of the fields to be updated</param>
            /// <param name="value">Updated values</param>
            /// <returns>The updated entity</returns>
            update : string -> string seq -> 'T -> Condition<'Y> seq -> 'T

            /// <summary>
            /// Delete records based on criteria
            /// </summary>
            /// <typeparam name="'Y">Type of the condition's value</typeparam>
            /// <param name="table">Name of the table</param>
            /// <param name="conditions">Criteria of deleting</param>
            /// <param name="value">Updated values</param>
            /// <returns>Number of affected rows</returns>
            delete : string -> Condition<'Y> seq -> int

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
            select : string -> Join<'Z> seq -> Condition<'Y> seq -> 'T list

            /// <summary>
            /// Execute a statement
            /// </summary>
            /// <param name="_param">The execute operation parameter</param>
            /// <returns>The number of affected rows</returns>
            execute : ExecuteParameter -> int

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
            selectSingle : string -> Join<'Z> seq -> Condition<'Y> seq -> 'T

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
            selectScalar : string -> AggregateOperation -> Condition<'Y> seq -> 'U

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
            selectWithRelation : string -> Join<'Z> seq -> Condition<'Y> seq -> Func<'T, 'U, 'P> -> 'P list

            /// <summary>
            /// Apply the proper configurations to the database connections
            /// </summary>
            configureDatabase : unit -> unit
        }

