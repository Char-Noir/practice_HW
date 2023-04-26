SELECT 
    YEAR(ord_datetime) AS year, 
    MONTH(ord_datetime) AS month, 
    g.gr_name AS group_name, 
    COUNT(*) AS order_qty
FROM 
    Orders o
JOIN 
    Analysis a ON o.ord_an = a.an_id
JOIN 
    Groups g ON a.an_group = g.gr_id
GROUP BY 
    YEAR(ord_datetime), 
    MONTH(ord_datetime), 
    g.gr_name
ORDER BY 
    YEAR(ord_datetime), 
    MONTH(ord_datetime), 
    g.gr_name


