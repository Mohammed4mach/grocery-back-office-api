namespace App.Interfaces

open System
open Infrastructure.Core.Types

/// <summary>
/// Contract determine what repositories should implement
/// </summary>
/// <typeparam name="'T">The entity related to the repo</typeparam>
type IRepository<'T when 'T : equality and 'T : null> =

    /// <summary>
    /// Get a collection of the resource
    /// </summary>
    /// <typeparam name="'Y">Conditions values type</typeparam>
    /// <typeparam name="'Z">Joins conditions values type</typeparam>
    /// <param name="joins">Sequence of table joins to perform</param>
    /// <param name="conditions">Sequence of conditions to filter the result</param>
    /// <returns>Sequence of the entity</returns>
    abstract member get<'U, 'P, 'Y, 'Z when 'Y : null and 'Z : null> : Join<'Z> seq -> Condition<'Y> seq -> 'T list

    /// <summary>
    /// Get a resource by its identifier
    /// </summary>
    /// <typeparam name="'Z">Joins conditions values type</typeparam>
    /// <param name="joins">Sequence of table joins to perform</param>
    /// <returns>The entity that match for the identifier</returns>
    abstract member find<'U, 'P, 'Z when 'Z : null> : string -> Join<'Z> seq -> 'T

    /// <summary>
    /// Get the resource by the provided conditions
    /// </summary>
    /// <typeparam name="'Y">Conditions values type</typeparam>
    /// <typeparam name="'Z">Joins conditions values type</typeparam>
    /// <param name="joins">Sequence of table joins to perform</param>
    /// <param name="conditions">The entity that match for the condition</param>
    /// <returns>Sequence of the entity</returns>
    abstract member findWhere<'U, 'P, 'Y, 'Z when 'Y : null and 'Z : null> : Join<'Z> seq -> Condition<'Y> seq -> 'T

    /// <summary>
    /// Get count of the resources that match the provided conditions
    /// </summary>
    /// <typeparam name="'Y">Conditions values type</typeparam>
    /// <param name="conditions">Sequence of conditions to filter the result</param>
    /// <returns>Count of the records that match the conditions</returns>
    abstract member count<'P, 'Y, 'Z when 'Y : null and 'Z : null> : Condition<'Y> seq -> int

    /// <summary>
    /// Store record of the resource
    /// </summary>
    /// <param name="value">The values to insert in the database</param>
    /// <returns>The inserted record</returns>
    abstract member store<'U, 'P, 'Y, 'Z when 'Y : null and 'Z : null> : 'T -> 'T

    /// <summary>
    /// Update values of the resource
    /// </summary>
    /// <param name="id">Identifier of the record</param>
    /// <param name="value">The resource with the updated properties</param>
    /// <returns>The udpated record</returns>
    abstract member update<'U, 'P, 'Z when 'Z : null> : string -> 'T -> 'T

    /// <summary>
    /// Update part of the resource
    /// </summary>
    /// <param name="id">Identifier of the record</param>
    /// <param name="fields">Name of the fields to update</param>
    /// <param name="value">The resource with the updated properties</param>
    /// <returns>The updated record</returns>
    abstract member partialUpdate<'U, 'P, 'Z when 'Z : null> : string -> string seq -> 'T -> 'T

    /// <summary>
    /// Delete record from the database
    /// </summary>
    /// <param name="id">Identifier of the record</param>
    abstract member delete<'U, 'P, 'Z when 'Z : null> : string -> unit

    /// <summary>
    /// Get a collection of the resource with a related entity
    /// </summary>
    /// <typeparam name="'U">Type of related entity</typeparam>
    /// <typeparam name="'Y">Conditions values type</typeparam>
    /// <typeparam name="'Z">Join condition values type</typeparam>
    /// <param name="joins">Sequence of table joins to perform</param>
    /// <param name="conditions">Sequence of conditions to filter the result</param>
    /// <param name="splitOn">Funciton that compose the returned entites into the returned one</param>
    /// <returns>Collection of the resource with the related entity</returns>
    abstract member getWithRelation<'U, 'Y, 'Z when 'Y : null and 'Z : null> : Join<'Z> seq -> Condition<'Y> seq -> Func<'T, 'U, 'T> -> 'T list

