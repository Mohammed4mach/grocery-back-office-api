namespace App.Services

open Core.Entities
open Infrastructure.Repositories
open Infrastructure.Core.Types

module DeliveryTimeRuleService =
    let index (filters : Condition seq) : DeliveryTimeRule seq =
        let rules = DeliveryTimeRuleRepository.get filters

        rules

    let show (id : string) : DeliveryTimeRule =
        let rule = DeliveryTimeRuleRepository.find id

        rule

    let store (rule : DeliveryTimeRule) : unit =
        DeliveryTimeRuleRepository.store rule

    let update (id : string) (updatedRule : DeliveryTimeRule) : unit =
        let rule = DeliveryTimeRuleRepository.find id

        DeliveryTimeRuleRepository.update id updatedRule

    let delete (id : string) : unit =
        DeliveryTimeRuleRepository.delete id

