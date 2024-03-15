
-- Create a new table called 'customer'
-- Drop the table if it already exists
IF OBJECT_ID('customer', 'U') IS NOT NULL
DROP TABLE customer
GO
-- Create the table 
CREATE TABLE customer
(
    TableNameId INT NOT NULL PRIMARY KEY, -- primary key column
    FirstName [NVARCHAR](50) NOT NULL,
    LastName [NVARCHAR](50) NOT NULL,
    HomeAddress [NVARCHAR](50) NOT NULL,
    coords [NVARCHAR](50) NOT NULL,
    DateCreated [DATE] NOT NULL
);
GO