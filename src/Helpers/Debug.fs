namespace Helpers

open Core.Exceptions

/// <summary>
/// Module that holds debugging helpers
/// </summary>
module Debug =

    /// <summary>
    /// Dumb data and die. It raise `Core.Exceptions.DumbAndDie` that can
    /// be handled with main error handler
    /// </summary>
    /// <param name="data">The data to be dumbed</param>
    let dd (data : obj) : unit =
        raise (DumbAndDie data)

