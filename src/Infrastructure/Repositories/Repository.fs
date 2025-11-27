namespace Infrastructure.Repositories

open System
open Core.Exceptions
open App.Interfaces
open Infrastructure.Core
open Infrastructure.Core.Types

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
        member this.get<'U, 'P, 'Y, 'Z when 'Y : null and 'Z : null> (joins : Join<'Z> seq) (conditions : Condition<'Y> seq) : 'T list =
            Database.operations<'T, 'U, 'P, 'Y, 'Z>.select this.table joins conditions

        member this.find<'U, 'P, 'Z when 'Z : null> (id : string) (joins : Join<'Z> seq) : 'T =
            let conditions = [ Helpers.Database.where this.identifier (Some id) ]

            let entity = Database.operations<'T, 'U, 'P, string, 'Z>.selectSingle this.table joins conditions

            if entity = null then
                raise (EntityNotFoundError $"Entity {this.table} of id = {id} not found")

            entity

        member this.findWhere<'U, 'P, 'Y, 'Z when 'Y : null and 'Z : null> (joins : Join<'Z> seq) (conditions : Condition<'Y> seq) : 'T =
            let entity = Database.operations<'T, 'U, 'P, 'Y, 'Z>.selectSingle this.table joins conditions

            if entity = null then
                raise (EntityNotFoundError $"Entity {this.table} of id = {id} not found")

            entity

        member this.count<'P, 'Y, 'Z when 'Y : null and 'Z : null> (conditions : Condition<'Y> seq) : int =
            Database.operations<'T, int, 'P, 'Y, 'Z>.selectScalar this.table (Helpers.Database.count "*") conditions

        member this.store<'U, 'P, 'Y, 'Z when 'Y : null and 'Z : null> (value : 'T) : 'T =
            Database.operations<'T, 'U, 'P, 'Y, 'Z>.insert this.table this.fillable value

        member this.update<'U, 'P, 'Z when 'Z : null> (id : string) (value : 'T) : 'T =
            let conditions = [ Helpers.Database.where this.identifier (Some id) ]

            Database.operations<'T, 'U, 'P, string, 'Z>.update this.table this.fillable value conditions

        member this.partialUpdate<'U, 'P, 'Z when 'Z : null> (id : string) (fields : string seq) (value : 'T) : 'T =
            let conditions = [ Helpers.Database.where this.identifier (Some id) ]

            Database.operations<'T, 'U, 'P, string, 'Z>.update this.table fields value conditions

        member this.delete<'U, 'P, 'Z when 'Z : null> (id : string) : unit =
            let conditions = [ Helpers.Database.where this.identifier (Some id) ]

            Database.operations<'T, 'U, 'P, string, 'Z>.delete this.table conditions |> ignore

        member this.getWithRelation<'U, 'Y, 'Z when 'Y : null and 'Z : null> (joins : Join<'Z> seq) (conditions: Condition<'Y> seq) (splitOn : System.Func<'T,'U,'T>): 'T list =
            Database.operations<'T, 'U, 'T, 'Y, 'Z>.selectWithRelation this.table joins conditions splitOn

