INSERT INTO delivery_time_rules (id, name, in_advance_days, same_day_deadline) VALUES
    (
        1,
        'In-Stock Rule',
        0,
        '18:00'
    ),
    (
        2,
        'Fresh Food Rule',
        0,
        '12:00'
    ),
    (
        3,
        'External Rule',
        3,
        NULL
    );

INSERT INTO delivery_time_rule_not_available_weekdays (delivery_time_rule_id, weekday_id) VALUES
    (/* In-Stock Rule - Saturday */
        1,
        1
    ),
    (/* In-Stock Rule - Sunday */
        1,
        2
    ),
    (/* Fresh Food Rule - Saturday */
        2,
        1
    ),
    (/* Fresh Food Rule - Sunday */
        2,
        2
    ),
    (/* External Rule - Saturday */
        3,
        1
    ),
    (/* External Rule - Sunday */
        3,
        2
    ),
    (/* External Rule - Monday */
        3,
        3
    );

