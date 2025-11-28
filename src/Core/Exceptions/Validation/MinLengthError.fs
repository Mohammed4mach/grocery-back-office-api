namespace Core.Exceptions.Validation

/// <summary>
/// Indicates that a string field length is less than a specific length
/// </summary>
/// <param name="message">The message of the exception</param>
exception MinLengthError of string

