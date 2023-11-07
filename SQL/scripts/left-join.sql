USE TestDB001
GO

--INSERT INTO TestSchema.TestTable (Name, Subscribed, CreatedDate, UpdatedDate, Balance, AGE)
--VALUES ('Marie Curie', 0, GETDATE(), GETDATE(), 5000.00, 86)

SELECT [Users].[ID],
       [Users].[Name],
       [Users].[AGE],
       [UsersExtra].[Description]
FROM  TestSchema.TestTable AS Users
    LEFT JOIN TestSchema.AnotherTable AS UsersExtra
        ON UsersExtra.TestTableID = Users.ID;
        