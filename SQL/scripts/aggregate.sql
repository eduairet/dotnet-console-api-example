USE TestDB001
GO

SELECT SUM(Balance) AS TotalBalance,
    STRING_AGG(Name, ', ') AS Names
FROM TestSchema.TestTable
Where Subscribed = 1
GROUP BY Name
ORDER BY TotalBalance DESC;