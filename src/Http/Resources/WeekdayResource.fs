namespace Http.Resources

open Core.Entities

type WeekdayResourceData =
    {
        id: int
        name: string
        code: string
    }

    static member ofEntity (weekday : Weekday) : WeekdayResourceData =
        {
            id   = weekday.id
            name = weekday.name
            code = weekday.code
        }

type WeekdayResource =
    {
        data : WeekdayResourceData
    }

    static member ofEntity (weekday : Weekday) : WeekdayResource =
        { data = WeekdayResourceData.ofEntity weekday }

