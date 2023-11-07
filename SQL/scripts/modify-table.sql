-- SET IDENTITY_INSERT TestSchema.TestTable ON -- Enables the use of IDENTITY columns
-- SET IDENTITY_INSERT TestSchema.TestTable OFF -- Disables the use of IDENTITY columns
USE TestDB001
GO

DELETE FROM TestSchema.TestTable -- Deletes all elements from the table
DELETE FROM TestSchema.TestTable WHERE ID = 1 -- Deletes the element with ID = 1

INSERT INTO TestSchema.TestTable (Name, Subscribed, CreatedDate, UpdatedDate, Balance, AGE)
VALUES ('John Doe', NULL, GETDATE(), GETDATE(), 100.00, 30), -- Inserts a new element in the table
       ('Alice Doe', NULL, GETDATE(), GETDATE(), 700.00, 18)

UPDATE TestSchema.TestTable -- Updates all elements in the table
SET Subscribed = 1, UpdatedDate = GETDATE() -- Updates the Subscribed and UpdatedDate columns
WHERE Name = 'John Doe'
GO

INSERT INTO TestSchema.AnotherTable (TestTableID, Description, SomeValue)
SELECT ID, 'Description for ' + CAST(ID AS NVARCHAR(10)), AGE
FROM TestSchema.TestTable;

SELECT Name,
       ISNULL(Subscribed, 0) as Subscribed, --Returns 0 if subscribed is null
       CreatedDate,
       UpdatedDate,
       Balance,
       AGE
  FROM TestSchema.TestTable
GO