namespace Core.Entities

open System

[<CLIMutable>]
[<CustomEquality; CustomComparison>]
type TimeSlot =
    {
        time : TimeOnly
        is_green : bool
    }

    member private this.Compare (anotherSlot : TimeSlot) =
        match this.is_green.CompareTo anotherSlot.is_green with
        | 0 -> this.time.CompareTo anotherSlot.time
        | cmp -> -cmp

    override this.Equals (obj : obj) : bool =
        match obj with
        | :? TimeSlot as anotherSlot -> (this.Compare anotherSlot) = 0
        | _ -> false

    override this.GetHashCode (): int =
        hash {| time = this.time; is_green = this.is_green |}

    interface IComparable with
        member this.CompareTo (obj : obj) : int =
            match obj with
            | :? TimeSlot as anotherSlot -> (this.Compare anotherSlot)
            | _ -> -1

