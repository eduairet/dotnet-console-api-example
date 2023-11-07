USE TestDB001
GO

-- CREATE CLUSTERED INDEX cix_ID_TestTableID
-- ON TestSchema.TestTable(ID)
-- If the index is already created, drop it first, since you can only have one

-- CREATE NONCLUSTERED INDEX ix_Name
-- ON TestSchema.TestTable(Name)

CREATE NONCLUSTERED INDEX fix_Subscribed ON TestSchema.TestTable(Subscribed)
    INCLUDE (ID, Name) WHERE Subscribed = 1;