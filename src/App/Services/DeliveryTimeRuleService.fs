namespace App.Services

open Core.Entities
open Infrastructure.Repositories
open Infrastructure.Core.Types

module DeliveryTimeRuleService =
    let index (filters : Condition seq) : DeliveryTimeRule seq =
        let rules = DeliveryTimeRuleRepository.get filters

        rules

    let show (id : int) : DeliveryTimeRule =
        let rule = DeliveryTimeRuleRepository.find (id.ToString())

        rule

    let store (rule : DeliveryTimeRule) : DeliveryTimeRule =
        DeliveryTimeRuleRepository.store rule

    let update (id : int) (updatedRule : DeliveryTimeRule) : DeliveryTimeRule =
        let rule = DeliveryTimeRuleRepository.find (id.ToString())

        DeliveryTimeRuleRepository.update (id.ToString()) updatedRule

    let delete (id : int) : unit =
        DeliveryTimeRuleRepository.delete (id.ToString())

