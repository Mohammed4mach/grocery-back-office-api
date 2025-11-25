namespace Http.Handlers

open System
open Microsoft.AspNetCore.Http
open Giraffe
open Helpers.Validation
open Core.Entities
open Infrastructure.Core.Types
open App.Services
open Http.Resources
open Http.Requests

module DeliveryTimeRuleHandlers =
    let index : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            let filters : Condition seq = []
            let rules  = DeliveryTimeRuleService.index filters
            let collection = DeliveryTimeRuleCollection.ofEntity rules

            negotiate collection next ctx

    let show (id : int) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            let rule, offdays = DeliveryTimeRuleService.show id
            let resource      = DeliveryTimeRuleWithRelationsResource.ofEntity rule offdays

            negotiate resource next ctx

    let store : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            bindModel<StoreDeliveryTimeRuleRequest> None (
                fun request ->
                    validate request

                    let sameDayDeadline =
                        match request.same_day_deadline = null with
                        | true -> new Nullable<TimeOnly>()
                        | false -> new Nullable<TimeOnly>(TimeOnly.Parse request.same_day_deadline)

                    let rule : DeliveryTimeRule = {
                        id                = 0
                        name              = request.name
                        in_advance_days   = request.in_advance_days
                        same_day_deadline = sameDayDeadline
                    }

                    let resource = DeliveryTimeRuleResource.ofEntity (DeliveryTimeRuleService.store rule)

                    negotiate resource
            ) next ctx

    let update (id : int) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            bindModel<UpdateDeliveryTimeRuleRequest> None (
                fun request ->
                    request.id <- id

                    validate request

                    let sameDayDeadline =
                        match request.same_day_deadline = null with
                        | true -> new Nullable<TimeOnly>()
                        | false -> new Nullable<TimeOnly>(TimeOnly.Parse request.same_day_deadline)

                    let rule : DeliveryTimeRule = {
                        id                = request.id
                        name              = request.name
                        in_advance_days   = request.in_advance_days
                        same_day_deadline = sameDayDeadline
                    }


                    let resource = DeliveryTimeRuleResource.ofEntity (DeliveryTimeRuleService.update id rule)

                    negotiate resource
            ) next ctx

    let delete (id : int) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            DeliveryTimeRuleService.delete id

            Successful.NO_CONTENT next ctx

    let addOffday (id : int) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            bindModel<StoreDeliveryTimeRuleNotAvailableWeekdayRequest> None (
                fun request ->
                    request.delivery_time_rule_id <- id

                    validate request

                    let resource =
                        DeliveryTimeRuleService.addOffday request.delivery_time_rule_id request.weekday_id |>
                        DeliveryTimeRuleNotAvailableWeekdayResource.ofEntity

                    negotiate resource
            ) next ctx

    let removeOffday (id : int) : HttpHandler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            DeliveryTimeRuleService.removeOffday id

            Successful.NO_CONTENT next ctx

