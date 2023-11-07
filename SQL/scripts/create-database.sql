USE master
GO

-- Immediately disconnect and delete the database
ALTER DATABASE TestDB001
SET SINGLE_USER
WITH ROLLBACK IMMEDIATE;

DROP DATABASE TestDB001
GO

-- Check if the database exists; if not, create it
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'TestDB001')
BEGIN
    CREATE DATABASE TestDB001
END
GO

USE TestDB001
GO

-- Check if the schema exists; if not, create it
IF NOT EXISTS (SELECT * FROM information_schema.schemata WHERE schema_name = 'TestSchema')
BEGIN
    EXEC('CREATE SCHEMA TestSchema')
END
GO

-- Check if the table exists; if not, create it
IF NOT EXISTS (SELECT * FROM information_schema.tables WHERE table_schema = 'TestSchema' AND table_name = 'TestTable')
BEGIN
    CREATE TABLE TestSchema.TestTable
    (
        ID INT IDENTITY(1,1) NOT NULL,
        Name NVARCHAR(100) NOT NULL,
        Subscribed BIT NULL,
        CreatedDate DATETIME NOT NULL,
        UpdatedDate DATETIME2 NOT NULL,
        Balance DECIMAL(10,2) NOT NULL,
        AGE INT NOT NULL,
        CONSTRAINT PK_TestTable PRIMARY KEY (ID)
    )
END
GO

IF NOT EXISTS (SELECT * FROM information_schema.tables WHERE table_schema = 'TestSchema' AND table_name = 'AnotherTable')
BEGIN
    CREATE TABLE TestSchema.AnotherTable
    (
        ID INT IDENTITY(1,1) NOT NULL,
        TestTableID INT NOT NULL, -- This column is used to establish the join with TestTable
        Description NVARCHAR(255) NULL,
        SomeValue INT NULL,
        CONSTRAINT PK_AnotherTable PRIMARY KEY (ID),
        CONSTRAINT FK_AnotherTable_TestTable FOREIGN KEY (TestTableID) REFERENCES TestSchema.TestTable(ID)
    )
END
GO


-- Now, you can run your SELECT statements
SELECT * FROM TestSchema.TestTable
ORDER BY ID DESC
GO

SELECT * FROM TestSchema.TestTable WHERE ID = 1
GO