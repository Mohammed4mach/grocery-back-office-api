namespace Core.Exceptions

/// <summary>
/// Debugging helper exception to dumb any object to the response then
/// stops the execution of the program. It must be handled with error handlers
/// </summary>
/// <param name="object">The object to be dumbed</param>
exception DumbAndDie of obj

