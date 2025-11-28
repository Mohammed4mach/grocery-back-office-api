namespace Core.Entities

/// <summary>
/// Entity that model product storage type
/// </summary>
[<CLIMutable>]
type ProductStorageType = {
    id: int
    name: string
    delivery_time_rule_id: int
}

