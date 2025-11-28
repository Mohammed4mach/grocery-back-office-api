namespace Core.Exceptions.Validation

/// <summary>
/// Indicates a business logic conflict
/// </summary>
/// <param name="message">The message of the exception</param>
exception ConflictError of string

