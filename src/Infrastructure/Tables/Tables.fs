namespace Infrastructure

open Dapper.FSharp.PostgreSQL
open Core.Entities

module Tables =
    let usersTable = table'<User> "users"
    let customersTable = table'<Customer> "customers"
    let ordersTable = table'<Order> "orders"
    let orderItemsTable = table'<OrderItem> "order_items"
    let productsTable = table'<Product> "products"
    let productStorageTypesTable = table'<ProductStorageType> "product_storage_types"
    let weekdaysTable = table'<Customer> "customer"
    let deliveryTimeRulesTable = table'<DeliveryTimeRule> "delivery_time_rules"
    let deliveryTimeRuleNotAvailableWeekdaysTable = table'<DeliveryTimeRuleNotAvailableWeekday> "delivery_time_rule_not_available_weekdays"

