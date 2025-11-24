namespace App.Services

open Core.Entities
open Infrastructure.Repositories
open Infrastructure.Core.Types

module DeliveryTimeRuleNotAvailableWeekdayService =
    let index (filters : Condition seq) : DeliveryTimeRuleNotAvailableWeekday seq =
        let notAvailableWeekdays = DeliveryTimeRuleNotAvailableWeekdayRepository.get filters

        notAvailableWeekdays

    let show (id : int) : DeliveryTimeRuleNotAvailableWeekday =
        let notAvailableWeekday = DeliveryTimeRuleNotAvailableWeekdayRepository.find (id.ToString())

        notAvailableWeekday

    let store (user : DeliveryTimeRuleNotAvailableWeekday) : DeliveryTimeRuleNotAvailableWeekday =
        DeliveryTimeRuleNotAvailableWeekdayRepository.store user

    let update (id : int) (updatedNotAvailableWeekday : DeliveryTimeRuleNotAvailableWeekday) : DeliveryTimeRuleNotAvailableWeekday =
        let notAvailableWeekday = DeliveryTimeRuleNotAvailableWeekdayRepository.find (id.ToString())

        DeliveryTimeRuleNotAvailableWeekdayRepository.update (id.ToString()) updatedNotAvailableWeekday

    let delete (id : int) : unit =
        DeliveryTimeRuleNotAvailableWeekdayRepository.delete (id.ToString())

