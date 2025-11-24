namespace Http.Resources

open Core.Entities

type WeekdayCollection =
    {
        data : WeekdayResourceData seq
    }

    static member ofEntity (weekdays : Weekday seq) : WeekdayCollection =
        let data : WeekdayResourceData seq =
            seq {
                for weekday in weekdays do
                    yield WeekdayResourceData.ofEntity weekday
            }

        { data = data }

