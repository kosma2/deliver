-- if this table exists, drop it
IF OBJECT_ID('member', 'U') IS NOT NULL
DROP TABLE member
GO

CREATE TABLE member
(
    MemberId [INT] IDENTITY(1,1) PRIMARY KEY,-- primary key with automatic numbering starting at 1
    Login [NVARCHAR](50) NOT NULL,
    PasswordHash [NVARCHAR](50) NOT NULL,
    Salt [VARBINARY](50) NOT NULL,
    Role [INT] NOT NULL,
);
GO