namespace Infrastructure.Repositories

[<AutoOpen>]
module ProductStorageType =
    type ProductStorageType = {
        id: int
        name: string
        deliveryTimeRuleId: string
    }

