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

    type Join =
        {
            _type : string
            table : string
            condition : Condition
        }

    type AggregateOperation =
        | Count of string
        | Sum of string
        | Avg of string
        | Min of string
        | Max of string

    type Operations<'T, 'U> =
        {
            insert : string -> string seq -> 'T -> 'T
            update : string -> string seq -> 'T -> Condition seq -> 'T
            delete : string -> Condition seq -> int
            select : string -> Join seq -> Condition seq -> 'T list
            execute : ExecuteParameter -> int
            selectSingle : string -> Join seq -> Condition seq -> 'T
            selectScalar : string -> AggregateOperation -> Condition seq -> 'U
            configureDatabase : unit -> unit
        }

