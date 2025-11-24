namespace App.Services

open Core.Entities
open Infrastructure.Repositories
open Infrastructure.Core.Types

module DeliveryTimeRuleNotAvailableWeekdayService =
    let index (filters : Condition seq) : DeliveryTimeRuleNotAvailableWeekday seq =
        let notAvailableWeekdays = DeliveryTimeRuleNotAvailableWeekdayRepository.get filters

        notAvailableWeekdays

    let show (id : string) : DeliveryTimeRuleNotAvailableWeekday =
        let notAvailableWeekday = DeliveryTimeRuleNotAvailableWeekdayRepository.find id

        notAvailableWeekday

    let store (user : DeliveryTimeRuleNotAvailableWeekday) : DeliveryTimeRuleNotAvailableWeekday =
        DeliveryTimeRuleNotAvailableWeekdayRepository.store user

    let update (id : string) (updatedNotAvailableWeekday : DeliveryTimeRuleNotAvailableWeekday) : DeliveryTimeRuleNotAvailableWeekday =
        let notAvailableWeekday = DeliveryTimeRuleNotAvailableWeekdayRepository.find id

        DeliveryTimeRuleNotAvailableWeekdayRepository.update id updatedNotAvailableWeekday

    let delete (id : string) : unit =
        DeliveryTimeRuleNotAvailableWeekdayRepository.delete id

