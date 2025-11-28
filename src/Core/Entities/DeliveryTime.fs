namespace Core.Entities

open System

[<CLIMutable>]
[<CustomEquality; CustomComparison>]
type DeliveryTime =
    {
        date : DateOnly
        time : TimeOnly
    }

    member private this.Compare (anotherTime : DeliveryTime) =
        match this.date.CompareTo anotherTime.date with
        | 0 -> (this.time :> IComparable).CompareTo anotherTime.time
        | cmp -> -cmp

    override this.Equals (obj : obj) : bool =
        match obj with
        | :? DeliveryTime as anotherTime -> (this.Compare anotherTime) = 0
        | _ -> false

    override this.GetHashCode (): int =
        hash {| date = this.date; time = this.time |}

    interface IComparable with
        member this.CompareTo (obj : obj) : int =
            match obj with
            | :? DeliveryTime as anotherTime -> (this.Compare anotherTime)
            | _ -> -1

