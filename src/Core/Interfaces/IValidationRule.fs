namespace Core.Interfaces

type IValidationRule =
    abstract member Validate : unit -> unit

