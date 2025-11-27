namespace Helpers

open Core.Exceptions

module Debug =
    let dd (data : obj) : unit =
        raise (DumbAndDie data)

