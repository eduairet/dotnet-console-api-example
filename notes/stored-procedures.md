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
