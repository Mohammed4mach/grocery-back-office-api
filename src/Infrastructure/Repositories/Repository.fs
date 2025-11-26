namespace Infrastructure.Repositories

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
        member this.get (joins : Join seq) (conditions : Condition seq) : 'T list =
            Database.operations.select this.table joins conditions

        member this.find (id : string) (joins : Join seq) : 'T =
            let conditions = [ Helpers.Database.where this.identifier (Some id) ]

            let entity = Database.operations.selectSingle this.table joins conditions

            if entity = null then
                raise (EntityNotFoundError $"Entity {this.table} of id = {id} not found")

            entity

        member this.findWhere (joins : Join seq) (conditions : Condition seq) : 'T =
            let entity = Database.operations.selectSingle this.table joins conditions

            if entity = null then
                raise (EntityNotFoundError $"Entity {this.table} of id = {id} not found")

            entity

        member this.count (conditions : Condition seq) : int =
            Database.operations<'T, int>.selectScalar this.table (Helpers.Database.count "*") conditions

        member this.store (value : 'T) : 'T =
            Database.operations.insert this.table this.fillable value

        member this.update (id : string) (value : 'T) : 'T =
            let conditions = [ Helpers.Database.where this.identifier (Some id) ]

            Database.operations.update this.table this.fillable value conditions

        member this.partialUpdate (id : string) (fields : string seq) (value : 'T) : 'T =
            let conditions = [ Helpers.Database.where this.identifier (Some id) ]

            Database.operations.update this.table fields value conditions

        member this.delete (id : string) : unit =
            let conditions = [ Helpers.Database.where this.identifier (Some id) ]

            Database.operations.delete this.table conditions |> ignore

