namespace Core.Entities

open System

/// <summary>
/// Entity that model days of the week resource
/// </summary>
[<CLIMutable>]
type Weekday = {
    id: int
    name: string
    code: string
}

