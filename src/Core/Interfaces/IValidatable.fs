namespace Core.Interfaces

/// <summary>
/// Contract that ensure that the request is validatable through
/// implementing `Rule` method
/// </summary>
type IValidatable =

    /// <summary>
    /// Get the rules of validation
    /// </summary>
    /// <returns>A sequence of validation rules</returns>
    abstract member Rules : unit -> IValidationRule seq

