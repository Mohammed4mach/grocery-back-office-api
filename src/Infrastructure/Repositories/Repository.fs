namespace Infrastructure.Repositories

open Core.Exceptions
open App.Interfaces
open Infrastructure.Core
open Infrastructure.Core.Types

/// <summary>
/// Implementation of `IRepository` contract, with fields to facilitate
/// its usage
/// </summary>
/// <typeparam name="'T">Type of the entity</typeparam>
/// <returns>returnDescription</returns>
type Repository<'T when 'T : equality and 'T : null> =
    {
        table : string
        fillable : string seq
        identifier : string
    }

    static member Default : Repository<'T> =
        {
            table      = "table"
            fillable   = []
            identifier = "id"
        }

    interface IRepository<'T> with
        /// <summary>
        /// Get a collection of the resource
        /// </summary>
        /// <typeparam name="'Y">Conditions values type</typeparam>
        /// <typeparam name="'Z">Joins conditions values type</typeparam>
        /// <param name="joins">Sequence of table joins to perform</param>
        /// <param name="conditions">Sequence of conditions to filter the result</param>
        /// <returns>Sequence of the entity</returns>
        member this.get<'U, 'P, 'Y, 'Z when 'Y : null and 'Z : null> (joins : Join<'Z> seq) (conditions : Condition<'Y> seq) : 'T list =
            Database.operations<'T, 'U, 'P, 'Y, 'Z>.select this.table joins conditions

        /// <summary>
        /// Get a resource by its identifier
        /// </summary>
        /// <typeparam name="'Z">Joins conditions values type</typeparam>
        /// <param name="joins">Sequence of table joins to perform</param>
        /// <returns>The entity that match for the identifier</returns>
        member this.find<'U, 'P, 'Z when 'Z : null> (id : string) (joins : Join<'Z> seq) : 'T =
            let conditions = [ Helpers.Database.where this.identifier (Some id) ]

            let entity = Database.operations<'T, 'U, 'P, string, 'Z>.selectSingle this.table joins conditions

            if entity = null then
                raise (EntityNotFoundError $"Entity {this.table} of id = {id} not found")

            entity

        /// <summary>
        /// Get the resource by the provided conditions
        /// </summary>
        /// <typeparam name="'Y">Conditions values type</typeparam>
        /// <typeparam name="'Z">Joins conditions values type</typeparam>
        /// <param name="joins">Sequence of table joins to perform</param>
        /// <param name="conditions">The entity that match for the condition</param>
        /// <returns>Sequence of the entity</returns>
        member this.findWhere<'U, 'P, 'Y, 'Z when 'Y : null and 'Z : null> (joins : Join<'Z> seq) (conditions : Condition<'Y> seq) : 'T =
            let entity = Database.operations<'T, 'U, 'P, 'Y, 'Z>.selectSingle this.table joins conditions

            if entity = null then
                raise (EntityNotFoundError $"Entity {this.table} of id = {id} not found")

            entity

        /// <summary>
        /// Get count of the resources that match the provided conditions
        /// </summary>
        /// <typeparam name="'Y">Conditions values type</typeparam>
        /// <param name="conditions">Sequence of conditions to filter the result</param>
        /// <returns>Count of the records that match the conditions</returns>
        member this.count<'P, 'Y, 'Z when 'Y : null and 'Z : null> (conditions : Condition<'Y> seq) : int =
            Database.operations<'T, int, 'P, 'Y, 'Z>.selectScalar this.table (Helpers.Database.count "*") conditions

        /// <summary>
        /// Store record of the resource
        /// </summary>
        /// <param name="value">The values to insert in the database</param>
        /// <returns>The inserted record</returns>
        member this.store<'U, 'P, 'Y, 'Z when 'Y : null and 'Z : null> (value : 'T) : 'T =
            Database.operations<'T, 'U, 'P, 'Y, 'Z>.insert this.table this.fillable value

        /// <summary>
        /// Update values of the resource
        /// </summary>
        /// <param name="id">Identifier of the record</param>
        /// <param name="value">The resource with the updated properties</param>
        /// <returns>The udpated record</returns>
        member this.update<'U, 'P, 'Z when 'Z : null> (id : string) (value : 'T) : 'T =
            let conditions = [ Helpers.Database.where this.identifier (Some id) ]

            Database.operations<'T, 'U, 'P, string, 'Z>.update this.table this.fillable value conditions

        /// <summary>
        /// Update part of the resource
        /// </summary>
        /// <param name="id">Identifier of the record</param>
        /// <param name="fields">Name of the fields to update</param>
        /// <param name="value">The resource with the updated properties</param>
        /// <returns>The updated record</returns>
        member this.partialUpdate<'U, 'P, 'Z when 'Z : null> (id : string) (fields : string seq) (value : 'T) : 'T =
            let conditions = [ Helpers.Database.where this.identifier (Some id) ]

            Database.operations<'T, 'U, 'P, string, 'Z>.update this.table fields value conditions

        /// <summary>
        /// Delete record from the database
        /// </summary>
        /// <param name="id">Identifier of the record</param>
        member this.delete<'U, 'P, 'Z when 'Z : null> (id : string) : unit =
            let conditions = [ Helpers.Database.where this.identifier (Some id) ]

            Database.operations<'T, 'U, 'P, string, 'Z>.delete this.table conditions |> ignore

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
        member this.getWithRelation<'U, 'Y, 'Z when 'Y : null and 'Z : null> (joins : Join<'Z> seq) (conditions: Condition<'Y> seq) (splitOn : System.Func<'T,'U,'T>): 'T list =
            Database.operations<'T, 'U, 'T, 'Y, 'Z>.selectWithRelation this.table joins conditions splitOn

