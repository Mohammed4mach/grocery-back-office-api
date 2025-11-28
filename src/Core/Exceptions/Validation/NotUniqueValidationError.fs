namespace Core.Exceptions.Validation

/// <summary>
/// Indicates field that is not unique among a specific resource field
/// </summary>
/// <param name="message">The message of the exception</param>
exception NotUniqueValidationError of string

