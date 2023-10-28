CREATE DATABASE TestDB001
GO

USE TestDB001
GO

CREATE SCHEMA TestSchema
GO

CREATE TABLE TestSchema.TestTable
(
    -- ID INT IDENTITY(StartPoint,IncrementBy) NOT NULL,
	ID INT IDENTITY(1,1) NOT NULL,
    -- No string type, instead it has:
        -- CHAR(numOfCharacters) - Fixed-length. It stores a specified number of characters, padding with spaces if necessary.
        -- VARCHAR(maxNumOfCharacters) - Stores a variable number of characters, only using as much storage as needed.
        -- NVARCHAR(maxNumOfCharacters) - Same as VARCHAR but it supports Unicode characters.
	Name NVARCHAR(100) NOT NULL,
    Subscribed BIT NOT NULL, -- Boolean 0 or 1 instead true or false
    CreatedDate DATETIME NOT NULL, -- Datetime object
    Balance DECIMAL(10,2) NOT NULL, -- Decimal object
    AGE INT NOT NULL, -- Integer object
    CONSTRAINT PK_TestTable PRIMARY KEY (ID) -- Forces the DB to create an ID for each entry
)
GO

SELECT * FROM TestSchema.TestTable -- Selects all elements in the table
GO

SELECT * FROM TestSchema.TestTable WHERE ID = 1 -- Selects the element with ID = 1
GO

-- DROP TABLE TestSchema.TestTable -- Command to delete the table