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
    -- WHERE ID BETWEEN 1 AND 10; -- You can use ranges with WHERE

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

    -   You can also manipulate the output of your queries

        ```SQL
        SELECT * FROM TestSchema.TestTable AS Users
        -- Aliasing elements from the table with the AS keyword makes it easier to handle

        SELECT [Users].[Name] + ' is ' + CONVERT(VARCHAR(4), [Users].[Age]) + ' years old' AS [User Info]
        FROM TestSchema.TestTable AS Users
        -- It will return a table with aliased "User Info" rows like this:
        -- John Doe is 30 years old
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

-   If several values share some columns they can be joined

    -   `JOIN` - Joins only compatible values (same as `INNER JOIn`)

        ```SQL
        USE TestDB001
        GO

        SELECT [Users].[ID],
            [Users].[Name],
            [Users].[AGE],
            [UsersExtra].[Description]
        FROM  TestSchema.TestTable AS Users
            JOIN TestSchema.AnotherTable AS UsersExtra
                ON UsersExtra.ID = Users.ID;

        -- 1	John Doe	30	Description for 1
        ```

    -   `LEFT JOIN` - Shows all of the values adding `NULL` to the fields that don't match

-   Faster join with `WHERE EXISTS`

    ```SQL
    SELECT [Users].[ID],
           [Users].[Name],
           [Users].[AGE]
    FROM TestSchema.TestTable AS Users
        WHERE EXISTS (SELECT *
            FROM TestSchema.AnotherTable AS UsersExtra
                WHERE UsersExtra.TestTableID = Users.ID)
            AND [Users].[AGE] <> 30; -- Not equal
    ```

-   `UNION` - Joining several tables with the same structure (skips duplicates)

    ```SQL
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
    UNION
        SELECT [ID],
            [Name],
            [Subscribed],
            [CreatedDate],
            [UpdatedDate],
            [Balance],
            [AGE]
        FROM [TestSchema].[TestTable]
    GO
    ```

    -   If you need duplicates replace `UNION` by `UNION ALL`

-   If you need to enhance your queries you can create an index

    -   `CLUSTERED` - Primary keys become our clustered index automatically
        ```SQL
        CREATE CLUSTERED INDEX cix_ID_TestTableID
        ON TestSchema.TestTable(ID)
        ```
    -   `NONCLUSTERED`
        ```SQL
        CREATE NONCLUSTERED INDEX ix_Name
        ON TestSchema.TestTable(Name)
        ```
        -   Non clustered index include the cluster indexes we've created as well
    -   Filter index
        ```SQL
        CREATE NONCLUSTERED INDEX fix_Subscribed ON TestSchema.TestTable(Subscribed)
            INCLUDE (ID, Name) WHERE Subscribed = 1;
        ```

-   You can aggregate elements by using `GROUP BY`

    ```SQL
    SELECT Name, SUM(Balance) AS TotalBalance
    FROM TestSchema.TestTable
    Where Subscribed = 1
    GROUP BY Name
    ORDER BY TotalBalance DESC;
    ```

    -   Most common functions used with aggregate are
        ```SQL
        SUM() -- Sum of selected elements
        MIN() -- Minimum value of selected elements
        MAX() -- Maximum value of selected elements
        AVG() -- Average value of selected elements
        COUNT() -- Number of selected elements
        STRING_AGG() -- Creates a string from all elements matching the selection
        ISNULL() -- Adds a custom value if the selection is null
        ```

-   `OUTER APPLY`

    -   Creates a table-valued function to each row of the outer table, and then join the results of the function with the outer table

        ```SQL
        CREATE TABLE Customers (
            CustomerID INT PRIMARY KEY,
            CustomerName NVARCHAR(100)
        );

        CREATE TABLE Orders (
            OrderID INT PRIMARY KEY,
            CustomerID INT,
            OrderDate DATE
        );

        SELECT c.CustomerID, c.CustomerName, o.OrderID, o.OrderDate
        FROM Customers c
        OUTER APPLY (
            SELECT TOP 1 OrderID, OrderDate
            FROM Orders
            WHERE CustomerID = c.CustomerID
            ORDER BY OrderDate DESC
        ) AS o;
        ```

        -   This query retrieves customer data from the `Customers`
        -   For each customer, retrieves their most recent order information (OrderID and OrderDate)
        -   It uses the `OUTER APPLY` operator to ensure that all customers from the `Customers` table are included in the result (similar to `LEFT JOIN`)
        -   If a customer has no orders, the `OrderID` and `OrderDate` columns in the result will be `NULL`
        -   This query provides a comprehensive list of customers along with their latest order details, regardless of whether they have placed any orders

    -   If you need to filter out the `NULL` values use `CROSS APPLY` instead `OUTER APPLY`

-   Dates on SQL

    ```SQL
    SELECT GETDATE(); -- Current date
    SELECT DATEADD(YEAR, -5, GETDATE()); -- 5 years ago
    SELECT DATEDIFF(MINUTE,  DATEADD(YEAR, -5, GETDATE()), GETDATE()); -- Positive
    SELECT DATEDIFF(MINUTE, GETDATE(),  DATEADD(YEAR, -5, GETDATE())); -- Negative
    ```

-   Alter table

    ```SQL
    CREATE TABLE Employees (
        EmployeeID INT PRIMARY KEY,
        FirstName NVARCHAR(50),
        LastName NVARCHAR(50),
        HireDate DATE
    );

    ALTER TABLE Employees
    ADD Salary DECIMAL(10, 2); -- Adds the salary column to the table
    ```
