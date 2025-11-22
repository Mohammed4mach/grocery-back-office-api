namespace Http.Resources

open Core.Entities

type WeekdayResourceData =
    {
        id: int
        name: string
        code: string
    }

type WeekdayResource =
    {
        data : WeekdayResourceData
    }

    static member ofEntity (weekday : Weekday) : WeekdayResource =
        let data : WeekdayResourceData = {
            id   = weekday.id
            name = weekday.name
            code = weekday.code
        }

        let resource : WeekdayResource = { data = data }

        resource

