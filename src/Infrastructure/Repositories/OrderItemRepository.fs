namespace Infrastructure.Repositories

module OrderItem =
    type OrderItem = {
        id: int
        costPerItem: float
        quantity: int
        productId: int
        orderId: int
    }

