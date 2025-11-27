namespace Infrastructure.Core

open System

module Types =
    type ExecuteParameter =
        | QueryOnly of string
        | WithParam of string * obj

    type Condition<'T> =
        {
            column : string
            operator : string
            value : 'T option
        }

    type Join<'T>=
        {
            _type : string
            table : string
            condition : Condition<'T>
        }

    type AggregateOperation =
        | Count of string
        | Sum of string
        | Avg of string
        | Min of string
        | Max of string

    type Operations<'T, 'U, 'P, 'Y, 'Z> =
        {
            insert : string -> string seq -> 'T -> 'T
            update : string -> string seq -> 'T -> Condition<'Y> seq -> 'T
            delete : string -> Condition<'Y> seq -> int
            select : string -> Join<'Z> seq -> Condition<'Y> seq -> 'T list
            execute : ExecuteParameter -> int
            selectSingle : string -> Join<'Z> seq -> Condition<'Y> seq -> 'T
            selectScalar : string -> AggregateOperation -> Condition<'Y> seq -> 'U
            selectWithRelation : string -> Join<'Z> seq -> Condition<'Y> seq -> Func<'T, 'U, 'P> -> 'P list
            configureDatabase : unit -> unit
        }

