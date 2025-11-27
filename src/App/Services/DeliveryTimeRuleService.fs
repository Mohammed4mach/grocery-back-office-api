namespace App.Services

open System
open Core.Entities
open Core.Exceptions.Validation
open App.Repositories
open App.Interfaces
open Infrastructure.Core.Types

module DeliveryTimeRuleService =
    let private repo        = DeliveryTimeRuleRepository :> IRepository<DeliveryTimeRule | null>
    let private weekdayRepo = WeekdayRepository :> IRepository<Weekday | null>
    let private offdayRepo  = DeliveryTimeRuleNotAvailableWeekdayRepository :> IRepository<DeliveryTimeRuleNotAvailableWeekday | null>

    let index (filters : Condition<'P> seq) : DeliveryTimeRule seq =
        let rules = repo.get [] filters

        rules

    let show (id : int) : DeliveryTimeRule * Weekday seq =
        let rule = repo.find (id.ToString()) []

        // Get weekdays
        let condition : Condition<string> = Helpers.Database.where "delivery_time_rule_not_available_weekdays.delivery_time_rule_id" (Some (rule.id.ToString()))
        let joinCondition : Condition<string>  = Helpers.Database.where "weekdays.id" (Some "delivery_time_rule_not_available_weekdays.weekday_id")
        let join : Join<string> = Helpers.Database.innerJoin "delivery_time_rule_not_available_weekdays" joinCondition

        let weekdays = weekdayRepo.get [join] [condition]

        rule, weekdays

    let store (rule : DeliveryTimeRule) : DeliveryTimeRule =
        repo.store rule

    let update (id : int) (updatedRule : DeliveryTimeRule) : DeliveryTimeRule =
        let rule = repo.find (id.ToString()) []

        repo.update (id.ToString()) updatedRule

    let delete (id : int) : unit =
        repo.delete (id.ToString())

    let addOffday (id : int) (weekdayId : int) : DeliveryTimeRuleNotAvailableWeekday =
        let rule    = repo.find (id.ToString()) []
        let weekday = weekdayRepo.find (weekdayId.ToString()) []

        // Check if the bond exists
        let conditions : Condition<string> seq = [
            Helpers.Database.where "delivery_time_rule_id" (Some (rule.id.ToString()))
            Helpers.Database.where "weekday_id" (Some (weekday.id.ToString()))
        ]

        let bondCount  = offdayRepo.count conditions
        let bondExists = bondCount > 0

        if bondExists then
            raise (ConflictError $"{rule.name} already has {weekday.name} in its offday list")

        let offdayBond : DeliveryTimeRuleNotAvailableWeekday =
            {
                id                    = 0
                delivery_time_rule_id = rule.id
                weekday_id            = weekday.id
            }

        offdayRepo.store offdayBond

    let removeOffday (id : int) : unit =
        offdayRepo.delete (id.ToString())

    // Get delivery time rules of the storage types
    let getDeliveryRulesOfStorageTypes (storageTypes : ProductStorageType seq) : DeliveryTimeRule seq =
        let rulesIds : string array = storageTypes |> Seq.map (fun _type -> _type.delivery_time_rule_id.ToString()) |> Seq.distinct |> Array.ofSeq

        repo.get [] [ Helpers.Database.whereIn "id" rulesIds ]

    // Some helpers funs to combine delivery rules into one rule field
    let private toSameDayDeadline = fun (rule : DeliveryTimeRule) -> rule.same_day_deadline
    let private forHavingValue    = fun (deadline : Nullable<TimeOnly>) -> deadline.HasValue
    let private toTimeValue       = fun (deadline : Nullable<TimeOnly>) -> deadline.Value

    // Combine the rules into one rule, and return all offdays for the rules
    let combineRules (rules : DeliveryTimeRule seq) : DeliveryTimeRule * Weekday seq =
        let offdays : Weekday seq      = rules |> WeekdayService.getOffdaysOfDeliveryRule
        let inAdvanceDays : int        = rules |> Seq.map (fun rule -> rule.in_advance_days) |> Seq.max
        let sameDayDeadlines : TimeOnly seq =
            rules |>
            Seq.map toSameDayDeadline |>
            Seq.filter forHavingValue |>
            Seq.map toTimeValue

        let sameDayDeadline : TimeOnly option =
            match Seq.isEmpty sameDayDeadlines with
            | true -> None
            | false -> Some (Seq.min sameDayDeadlines)

        let rule : DeliveryTimeRule = {
            DeliveryTimeRule.Default with
                in_advance_days   = inAdvanceDays
                same_day_deadline =
                    match sameDayDeadline with
                    | Some deadline -> Nullable(deadline)
                    | None -> Nullable()
        }

        rule, offdays


