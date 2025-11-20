namespace Helpers

open Infrastructure.Core.Types

module Database =
    let makeCondition (column : string) (value : string option) (_operator : string option) : Condition =
        let operator = defaultArg _operator "="
        let condition : Condition = { column = column; operator = operator; value = value }

        condition

