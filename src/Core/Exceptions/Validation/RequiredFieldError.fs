namespace Core.Exceptions.Validation

/// <summary>
/// Indicates a required field error
/// </summary>
/// <param name="message">The message of the exception</param>
exception RequiredFieldError of string

