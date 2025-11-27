namespace Http.Handlers

open Microsoft.AspNetCore.Http
open Giraffe
open Helpers.Validation
open Core.Entities
open Infrastructure.Core.Types
open App.Services
open Http.Resources
open Http.Requests

module WeekdayHandlers =
    let index : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            let filters : Condition<string> seq = []
            let weekdays  = WeekdayService.index filters
            let collection = WeekdayCollection.ofEntity weekdays

            negotiate collection next ctx

    let show (id : int) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            let weekday = WeekdayService.show id
            let resource = WeekdayResource.ofEntity weekday

            negotiate resource next ctx

    let store : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            bindModel<StoreWeekdayRequest> None (
                fun request ->
                    validate request

                    let weekday : Weekday = {
                        id   = 0
                        name = request.name
                        code = request.code
                    }

                    let resource = WeekdayResource.ofEntity (WeekdayService.store weekday)

                    negotiate resource
            ) next ctx

    let update (id : int) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            bindModel<UpdateWeekdayRequest> None (
                fun request ->
                    request.id <- id

                    validate request

                    let weekday : Weekday = {
                        id   = request.id
                        name = request.name
                        code = request.code
                    }

                    let resource = WeekdayResource.ofEntity (WeekdayService.update id weekday)

                    negotiate resource
            ) next ctx

    let delete (id : int) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            WeekdayService.delete id

            Successful.NO_CONTENT next ctx

