namespace Infrastructure.Repositories

open Infrastructure.Core
open Infrastructure.Core.Types

type Repository<'T> =
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

    member this.get (conditions : Condition seq) :  'T list =
        Database.operations.select this.table conditions

    member this.find (id : string) : 'T =
        let conditions = [ Helpers.Database.where this.identifier (Some id) ]

        let entity = Database.operations.selectSingle this.table conditions

        entity

    member this.store (value : 'T) : unit =
        Database.operations.insert this.table this.fillable value |> ignore

    member this.update (id : string) (value : 'T) : unit =
        let conditions = [ Helpers.Database.where this.identifier (Some id) ]

        Database.operations.update this.table this.fillable value conditions |> ignore

    member this.delete (id : string) : unit =
        let conditions = [ Helpers.Database.where this.identifier (Some id) ]

        Database.operations.delete this.table conditions |> ignore

