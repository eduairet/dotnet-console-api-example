# Stored Procedures

-   Stored procedures are a set of SQL statements that are stored in the database and can be called by name.

    ```SQL
    USE DotNetCourseDatabase
    GO

    CREATE PROCEDURE TutorialAppSchema.sp_Get
    -- EXEC TutorialAppSchema.sp_Get
    AS
    BEGIN
        SELECT Users.UserId,
            Users.FirstName,
            Users.LastName,
            Users.Email,
            Users.Gender,
            Users.Active
        FROM TutorialAppSchema.Users AS Users
    END

    -- For modifying it use: ALTER PROCEDURE TutorialAppSchema.sp_Get
    ```

-   We can add parameters to the procedures

    ```SQL
    USE DotNetCourseDatabase
    GO

    ALTER PROCEDURE TutorialAppSchema.spUsers_Get
        -- EXEC TutorialAppSchema.sp_Get @UserId = 1 OR EXEC TutorialAppSchema.sp_Get 1 first approach is less prone to errors
        @UserId INT
    AS
    BEGIN
        SELECT Users.UserId,
            Users.FirstName,
            Users.LastName,
            Users.Email,
            Users.Gender,
            Users.Active
        FROM TutorialAppSchema.Users AS Users
        WHERE Users.UserId = @UserId
    END
    ```

-   We can also add default values to the parameters

    ```SQL
    @UserId INT = NULL -- Making it NULL will allows to handle the case when the parameter is not passed
    --
    WHERE Users.UserId = ISNULL(@UserId, Users.UserId) -- If the parameter is not passed then the UserId will be used
    ```

-   This can allow us to create even more complex logic with a simple execution, like querying from different tables

    ```SQL
    USE DotNetCourseDatabase
    GO

    ALTER PROCEDURE TutorialAppSchema.spUsers_Get
        /* EXEC TutorialAppSchema.sp_Get @UserId = 1 */
        @UserId INT = NULL
    AS
    BEGIN
        SELECT Users.UserId,
            Users.FirstName,
            Users.LastName,
            Users.Email,
            Users.Gender,
            Users.Active,
            UserJobInfo.Department,
            UserJobInfo.JobTitle,
            UserSalary.Salary,
            UserAvgSalary.AvgSalary
        FROM TutorialAppSchema.Users AS Users
            LEFT JOIN TutorialAppSchema.UserSalary AS UserSalary
            ON UserSalary.UserId = Users.UserId
            LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo
            ON UserJobInfo.UserId = Users.UserId
            OUTER APPLY (
                SELECT AVG(UserSalary2.Salary) AS AvgSalary
            FROM TutorialAppSchema.Users AS Users
                LEFT JOIN TutorialAppSchema.UserSalary AS UserSalary2
                ON UserSalary2.UserId = Users.UserId
                LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo2
                ON UserJobInfo2.UserId = Users.UserId
            WHERE UserJobInfo.Department = UserJobInfo2.Department
            GROUP BY UserJobInfo2.Department
            ) AS UserAvgSalary
        WHERE Users.UserId = ISNULL(@UserId, Users.UserId)
    END
    ```

-   Temporary tables

    -   Temporary tables are tables that are created in the tempdb database and are automatically deleted when they are no longer used.
    -   They are useful when we need to store data temporarily, for example, when we need to store the result of a query to use it later.
    -   There are two types of temporary tables:
        -   Local temporary tables: They are only visible in the current session and are deleted when the session is closed.
        -   Global temporary tables: They are visible to all sessions and are deleted when the last session that uses them is closed.
    -   ```SQL
        USE DotNetCourseDatabase
        GO

        ALTER PROCEDURE TutorialAppSchema.spUsers_Get
            @UserId INT = NULL
        AS
        BEGIN
            DROP TABLE IF EXISTS #AvgDeptSalary
            -- Drop the temp table if it exists to avoid errors
            /* Older versions of SQL Server do not support the IF EXISTS clause
            IF OBJECT_ID('tempdb..#AvgDeptSalary', 'U') IS NOT NULL
                BEGIN
                    DROP TABLE #AvgDeptSalary
                END
            */

            SELECT UserJobInfo.Department
                , AVG(UserSalary.Salary) AS AvgSalary
            INTO #AvgDeptSalary
            -- # = Local temp table | ## = Global temp table
            FROM TutorialAppSchema.Users AS Users
                LEFT JOIN TutorialAppSchema.UserSalary AS UserSalary
                ON UserSalary.UserId = Users.UserId
                LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo
                ON UserJobInfo.UserId = Users.UserId
            GROUP BY UserJobInfo.Department

            /* To make querying in the new table faster we can create a new clustered index */
            CREATE CLUSTERED INDEX cix_#AvgDeptSalary_Department ON #AvgDeptSalary(Department)

            SELECT Users.UserId,
                Users.FirstName,
                Users.LastName,
                Users.Email,
                Users.Gender,
                Users.Active,
                UserJobInfo.Department,
                UserJobInfo.JobTitle,
                UserSalary.Salary,
                UserAvgSalary.AvgSalary
            FROM TutorialAppSchema.Users AS Users
                LEFT JOIN TutorialAppSchema.UserSalary AS UserSalary
                ON UserSalary.UserId = Users.UserId
                LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo
                ON UserJobInfo.UserId = Users.UserId
                LEFT JOIN #AvgDeptSalary AS UserAvgSalary
                ON UserAvgSalary.Department = UserJobInfo.Department
            WHERE Users.UserId = ISNULL(@UserId, Users.UserId)
            AND ISNULL(Users.Active, 0) = ISNULL(@Active, ISNULL(Users.Active, 0))
            -- or: AND ISNULL(Users.Active, 0) = COALESCE(@Active, Users.Active, 0)
        END
        ```
