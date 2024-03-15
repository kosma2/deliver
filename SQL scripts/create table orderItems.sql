-- if this table exists, drop it
IF OBJECT_ID('orderItems', 'U') IS NOT NULL
DROP TABLE orderItems
GO

CREATE TABLE orderItems
(
    OrderItemId [INT] IDENTITY(1,1) PRIMARY KEY,-- primary key with automatic numbering starting at 1
    OrderId [INT] NOT NULL,
    ItemId [INT] NOT NULL,
    Quantity [INT] NOT NULL,
    Price [DECIMAL](5,2) NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES orders(OrderId), -- refers to the order that this item belongs to
    FOREIGN KEY (ItemId) REFERENCES inventory(TableNameId) --id in the iventory table
);
GO