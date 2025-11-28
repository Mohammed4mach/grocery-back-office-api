namespace Core.Exceptions

/// <summary>
/// Indicates that a specific record of a resource is not found
/// </summary>
/// <param name="message">The message of the exception</param>
exception EntityNotFoundError of string

