namespace Http.Requests

open Core.Interfaces
open Core.ValidationRules

[<CLIMutable>]
type StoreDeliveryTimeRuleNotAvailableWeekdayRequest =
    {
        delivery_time_rule_id: int
        weekday_id: int
    }

    interface IValidatable with
        member this.Rules (): IValidationRule seq =
            [
                (* Rule exists *)
                new Required<int>("delivery_time_rule_id", this.delivery_time_rule_id);
                new Exists<int>("delivery_time_rule_id", this.delivery_time_rule_id, "delivery_time_rules", "id")
                (* Weekday exists *)
                new Required<int>("weekday_id", this.weekday_id);
                new Exists<int>("weekday_id", this.weekday_id, "weekdays", "id")
            ]

