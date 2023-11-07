USE TestDB001
GO

SELECT [Users].[ID],
       [Users].[Name],
       [Users].[AGE],
       [UsersExtra].[Description]
FROM  TestSchema.TestTable AS Users
    JOIN TestSchema.AnotherTable AS UsersExtra
        ON UsersExtra.TestTableID = Users.ID;
        