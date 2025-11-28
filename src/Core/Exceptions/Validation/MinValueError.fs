namespace Core.Exceptions.Validation

/// <summary>
/// Indicates that a field is below its minimum value
/// </summary>
/// <param name="message">The message of the exception</param>
exception MinValueError of string

