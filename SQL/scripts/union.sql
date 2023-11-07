USE TestDB001
GO

    SELECT [ID],
        [Name],
        [Subscribed],
        [CreatedDate],
        [UpdatedDate],
        [Balance],
        [AGE]
    FROM [TestSchema].[TestTable]
UNION ALL
    SELECT [ID],
        [Name],
        [Subscribed],
        [CreatedDate],
        [UpdatedDate],
        [Balance],
        [AGE]
    FROM [TestSchema].[TestTable]
GO