# Grocery Back-Office API

Table of Contents
=================

- [Installation](#installation)
- [Entity Relationship Diagram](#entity-relationship-diagram)
- [ERD Mapping](#erd-mapping)

## Installation

To install and run the Grocery Back-Office API, follow these steps:
1. Clone the repository:

```bash
    git clone
```

## Entity Relationship Diagram

[![ERD](assets/diagrams/ERD.png)](assets/diagrams/ERD.png)

## ERD Mapping

### user

| Column | Type | Constraints |
|--------|------|-------------|
| id | integer | Primary Key |
| fullname | varchar | |
| username | varchar | |
| password | varchar | |

### product

| Column | Type | Constraints |
|--------|------|-------------|
| id | integer | Primary Key |
| name | varchar | |
| price | decimal | |
| description | text | |
| product_storage_type_id | integer | References product_storage_type(id) |

### product_storage_type

| Column | Type | Constraints |
|--------|------|-------------|
| id | integer | Primary Key |
| name | varchar | |
| delivery_time_rule_id | integer | References delivery_time_rule(id) |

### delivery_time_rule

| Column | Type | Constraints |
|--------|------|-------------|
| id | integer | Primary Key |
| name | varchar | |
| in_advance_days | integer | |
| same_day_deadline | time | |

### weekday

| Column | Type | Constraints |
|--------|------|-------------|
| id | integer | Primary Key |
| name | varchar | |
| code | varchar | |

### delivery_time_rule_not_available_weekday (pivot table)

| Column | Type | Constraints |
|--------|------|-------------|
| id | integer | Primary Key |
| delivery_time_rule_id | integer | References delivery_time_rule(id) |
| weekday_id | integer | References weekday(id) |

### customer

| Column | Type | Constraints |
|--------|------|-------------|
| id | integer | Primary Key |
| fullname | varchar | |
| address | text | |

### order

| Column | Type | Constraints |
|--------|------|-------------|
| id | integer | Primary Key |
| total_cost | decimal | |
| order_time | timestamp | |
| delivery_date | date | |
| delivery_time | time | |
| is_green_delivery | boolean | |
| user_id | integer | References user(id) |
| customer_id | integer | References customer(id) |

### order_item

| Column | Type | Constraints |
|--------|------|-------------|
| id | integer | Primary Key |
| cost_per_item | decimal | |
| quantity | integer | |
| product_id | integer | References product(id) |
| order_id | integer | References order(id) |

