
-- Create a new table called 'customer' in schema 'dbo'
-- Drop the table if it already exists
IF OBJECT_ID('dbo.customer', 'U') IS NOT NULL
DROP TABLE dbo.customer
GO
-- Create the table in the specified schema
CREATE TABLE dbo.customer
(
    TableNameId INT NOT NULL PRIMARY KEY, -- primary key column
    FirstName [NVARCHAR](50) NOT NULL,
    LastName [NVARCHAR](50) NOT NULL,
    HomeAddress [NVARCHAR](50) NOT NULL,
    coords [NVARCHAR](50) NOT NULL,
    DateCreated [DATE] NOT NULL
);
CREATE TABLE dbo.inventory
(
    TableNameId INT NOT NULL PRIMARY KEY,
    ItemName [NVARCHAR](50) NOT NULL,
    ItemDescr [NVARCHAR](50) NOT NULL,
    ItemPrice[SMALLMONEY] NOT NULL,
    ItemDiment[NVARCHAR](50) NOT NULL,
    ItemWeight[DECIMAL](3) NOT NULL,
)
GO