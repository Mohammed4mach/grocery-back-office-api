CREATE VIEW products_view AS
    SELECT
        p.id,
        p.name,
        p.price,
        p.description,
        p.product_storage_type_id,
        t.name as product_storage_type_name
    FROM
        products p
    INNER JOIN
        product_storage_types t
    ON
        p.product_storage_type_id = t.id

