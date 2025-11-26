namespace App.Services

open Core.Entities
open Core.Exceptions.Validation
open App.Repositories
open App.Interfaces
open Infrastructure.Core.Types

module DeliveryTimeRuleService =
    let private repo        = DeliveryTimeRuleRepository :> IRepository<DeliveryTimeRule | null>
    let private weekdayRepo = WeekdayRepository :> IRepository<Weekday | null>
    let private offdayRepo  = DeliveryTimeRuleNotAvailableWeekdayRepository :> IRepository<DeliveryTimeRuleNotAvailableWeekday | null>

    let index (filters : Condition seq) : DeliveryTimeRule seq =
        let rules = repo.get [] filters

        rules

    let show (id : int) : DeliveryTimeRule * Weekday seq =
        let rule = repo.find (id.ToString()) []

        // Get weekdays
        let condition : Condition = Helpers.Database.where "delivery_time_rule_not_available_weekdays.delivery_time_rule_id" (Some (rule.id.ToString()))
        let joinCondition : Condition  = Helpers.Database.where "weekdays.id" (Some "delivery_time_rule_not_available_weekdays.weekday_id")
        let join : Join = Helpers.Database.innerJoin "delivery_time_rule_not_available_weekdays" joinCondition

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
        let conditions : Condition seq = [
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

