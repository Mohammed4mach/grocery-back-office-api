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
            insert : string -> string seq -> 'T -> int
            update : string -> string seq -> 'T -> Condition seq -> int
            delete : string -> Condition seq -> int
            select : string -> Condition seq -> 'T list
            execute : ExecuteParameter -> int
            selectSingle : string -> Condition seq -> 'T
            configureDatabase : unit -> unit
        }

