namespace Infrastructure.Core

module Types =
    type ExecuteParameter =
        | QueryOnly of string
        | WithParam of string * obj

    type Operations =
        {
            configureDatabase : unit -> unit
            execute : ExecuteParameter -> int
            executeNonQuery : string -> int
        }

