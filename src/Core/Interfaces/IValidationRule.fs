namespace Core.Interfaces

/// <summary>
/// Ensure that a rule holds its validation logic through the implementation
/// of `Validate` member
/// </summary>
type IValidationRule =

    /// <summary>
    /// Apply the validation logic of the rule
    /// </summary>
    abstract member Validate : unit -> unit

