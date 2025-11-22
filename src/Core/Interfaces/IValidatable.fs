namespace Core.Interfaces

type IValidatable =
    abstract member Rules : unit -> IValidationRule seq

