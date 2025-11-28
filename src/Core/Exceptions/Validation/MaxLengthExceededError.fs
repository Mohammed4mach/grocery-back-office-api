namespace Core.Exceptions.Validation

/// <summary>
/// Indicates that a string field's maximum length is exceeded
/// </summary>
/// <param name="message">The message of the exception</param>
exception MaxLengthExceededError of string

