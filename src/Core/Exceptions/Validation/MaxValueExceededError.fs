namespace Core.Exceptions.Validation

/// <summary>
/// Indicates that field's maximum value is exceeded
/// </summary>
/// <param name="message">The message of the exception</param>
exception MaxValueExceededError of string

