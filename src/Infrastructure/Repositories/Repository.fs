namespace Infrastructure.Repository

type Repository<'T> =
    {
        get: Map<string, string> option -> List<'T>
        find: string -> 'T
        insert: 'T -> unit
        update: string * 'T -> unit
        delete: string -> unit
    }

