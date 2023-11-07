USE TestDB001
GO

SELECT [Users].[Name] + ' is ' + CONVERT(VARCHAR(4), [Users].[Age]) + ' years old' AS [User Info]
FROM TestSchema.TestTable AS Users