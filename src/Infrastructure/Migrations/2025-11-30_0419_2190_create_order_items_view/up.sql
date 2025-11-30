CREATE VIEW order_items_view AS
    SELECT
        i.id,
        i.cost_per_item,
        i.quantity,
        i.product_id,
        i.order_id,
        p.name as product_name
    FROM
        order_items i
    INNER JOIN
        products p
    ON
        i.product_id = p.id

