namespace Core.Exceptions.Validation

/// <summary>
/// Indicates a general bad request error
/// </summary>
/// <param name="message">The message of the exception</param>
exception BadRequestError of string

