# SQL

## SQL Scripting

-   Create and select database

    ```SQL
    CREATE DATABASE TestDB001
    GO
    ```

-   Create and populate schema (how the table will look like)

    ```SQL
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
        UpdatedDate DATETIME2 NOT NULL, -- Datetime2 object. It has a precision of 3 milliseconds.
        Balance DECIMAL(10,2) NOT NULL, -- Decimal object with 10 digits and 2 decimals
        AGE INT NOT NULL, -- Integer object
        CONSTRAINT PK_TestTable PRIMARY KEY (ID) -- Forces the DB to create an ID for each entry
    )
    GO
    ```

-   Select elements from the database

    ```SQL
    SELECT * FROM TestSchema.TestTable -- Selects all elements in the table
    GO

    SELECT * FROM TestSchema.TestTable WHERE ID = 1 -- Selects the element with ID = 1
    GO

    -- Controlled selections
    SELECT Name,
           ISNULL(Subscribed, 0) as Subscribed, --Returns 0 if subscribed is null
           CreatedDate,
           UpdatedDate,
           Balance,
           AGE
       FROM TestSchema.TestTable
       GO

    -- Order selection
    SELECT * FROM TestSchema.TestTable
    ORDER BY ID DESC, UpdatedDate DESC -- From bottom to top considering both fields
    GO
    ```

-   Delete the database

    ```SQL
    DROP TABLE TestSchema.TestTable -- Command to delete the table
    ```

-   Insert an element in the table

    ```SQL
    -- SET IDENTITY_INSERT TestSchema.TestTable ON -- Enables the use of IDENTITY columns
    -- SET IDENTITY_INSERT TestSchema.TestTable OFF -- Disables the use of IDENTITY columns
    INSERT INTO TestSchema.TestTable (Name, Subscribed, CreatedDate, UpdatedDate, Balance, AGE)
    VALUES ('John Doe', 1, GETDATE(), GETDATE(), 100.00, 30) -- Inserts a new element in the table
    ```

-   Delete elements from the table

    ```SQL
    DELETE FROM TestSchema.TestTable -- Deletes all elements from the table
    DELETE FROM TestSchema.TestTable WHERE ID = 1 -- Deletes the element with ID = 1
    ```

-   Update elements

    ```SQL
    UPDATE TestSchema.TestTable -- Updates all elements in the table
    SET Subscribed = 1, UpdatedDate = GETDATE() -- Updates the Subscribed and UpdatedDate columns
    WHERE Name = 'John Doe'
    GO
    ```
