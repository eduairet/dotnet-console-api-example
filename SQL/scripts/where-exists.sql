USE TestDB001
GO

SELECT [Users].[ID],
    [Users].[Name],
    [Users].[AGE]
FROM TestSchema.TestTable AS Users
WHERE EXISTS (SELECT *
    FROM TestSchema.AnotherTable AS UsersExtra
    WHERE UsersExtra.TestTableID = Users.ID)
    AND [Users].[AGE] <> 30; -- Not equal