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
