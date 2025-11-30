CREATE VIEW orders_view AS
    SELECT
        o.id,
        o.total_cost,
        o.order_time,
        o.delivery_date,
        o.delivery_time,
        o.is_green_delivery,
        o.user_id,
        o.customer_id,
        u.fullname as user_name,
        c.fullname as customer_name
    FROM
        orders o
    INNER JOIN
        users u
    ON
        o.user_id = u.id
    INNER JOIN
        customers c
    ON
        o.customer_id = c.id

