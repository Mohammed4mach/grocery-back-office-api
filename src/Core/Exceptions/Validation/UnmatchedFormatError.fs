namespace Core.Exceptions.Validation

/// <summary>
/// Indicates a field is not matching a specific format
/// </summary>
/// <param name="message">The message of the exception</param>
exception UnmatchedFormatError of string

