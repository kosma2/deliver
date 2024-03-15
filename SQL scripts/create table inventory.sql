-- Create a new table called 'inventory'
-- Drop the table if it already exists
IF OBJECT_ID('inventory', 'U') IS NOT NULL
DROP TABLE inventory
GO

-- Create the table in the specified schema
CREATE TABLE dbo.inventory
(
    TableNameId INT NOT NULL PRIMARY KEY,
    ItemName [NVARCHAR](50) NOT NULL,
    ItemDescr [NVARCHAR](50) NOT NULL,
    ItemPrice[SMALLMONEY] NOT NULL,
    ItemDiment[NVARCHAR](50) NOT NULL,
    ItemWeight[DECIMAL](3) NOT NULL,
);
GO