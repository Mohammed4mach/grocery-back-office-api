<h1 style="text-align: center">Grocery Back-Office API</h1>

## Table of Contents

- [Installation](#installation)
- [The Design](#the-design)
- [ERD Mapping](#erd-mapping)

## Installation

To install and run the Grocery Back-Office API, follow these steps:
1. Clone the repository:

```bash
git clone https://github.com/Mohammed4mach/grocery-back-office-api.git
```

2. Initialize the project (using [GNU Make](https://www.gnu.org/software/make/#download))

```bash
make project
```

3. Run the server

```bash
make
```

or use

```bash
dotnet run watch
```

**Note**:

**The project still under development and zero endpoint are implemented**

## The Design

Here is the design for my solution to the problem

[![ERD](assets/diagrams/ERD.png)](assets/diagrams/ERD.png)

The idea is to avoid hard-coding product categories and thier time constraints.
This is better for users and developers as business needs and policies tends
to change or be extended over the time.

## ERD Mapping

### Users

| Column | Type | Constraints |
|--------|------|-------------|
| id | integer | Primary Key |
| fullname | varchar | |
| username | varchar | |
| password | varchar | |

### Customers

| Column | Type | Constraints |
|--------|------|-------------|
| id | integer | Primary Key |
| fullname | varchar | |
| address | text | |

### Weekdays

| Column | Type | Constraints |
|--------|------|-------------|
| id | integer | Primary Key |
| name | varchar | |
| code | varchar | |

### Delivery Time Rules

| Column | Type | Constraints |
|--------|------|-------------|
| id | integer | Primary Key |
| name | varchar | |
| in_advance_days | integer | |
| same_day_deadline | time | |

### Delivery Time Rule Not Available Weekdays (pivot table)

| Column | Type | Constraints |
|--------|------|-------------|
| id | integer | Primary Key |
| delivery_time_rule_id | integer | References delivery_time_rule(id) |
| weekday_id | integer | References weekday(id) |

### Product Storage Types

| Column | Type | Constraints |
|--------|------|-------------|
| id | integer | Primary Key |
| name | varchar | |
| delivery_time_rule_id | integer | References delivery_time_rule(id) |

### Products

| Column | Type | Constraints |
|--------|------|-------------|
| id | integer | Primary Key |
| name | varchar | |
| price | decimal | |
| description | text | |
| product_storage_type_id | integer | References product_storage_type(id) |

### Orders

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

### Order Items

| Column | Type | Constraints |
|--------|------|-------------|
| id | integer | Primary Key |
| cost_per_item | decimal | |
| quantity | integer | |
| product_id | integer | References product(id) |
| order_id | integer | References order(id) |

