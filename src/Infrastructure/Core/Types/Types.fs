namespace Infrastructure.Core

module Types =
    type ExecuteParameter =
        | QueryOnly of string
        | WithParam of string * obj

    type Condition =
        {
            column : string
            operator : string
            value : string option
        }

    type Operations<'T> =
        {
            configureDatabase : unit -> unit
            execute : ExecuteParameter -> int
            insert : string -> string seq -> 'T seq -> Condition seq -> int
            update : string -> string seq -> 'T seq -> Condition seq -> int
            delete : string -> Condition seq -> int
            select : string -> Condition seq -> 'T list
            selectSingle : string -> Condition seq -> 'T
        }

