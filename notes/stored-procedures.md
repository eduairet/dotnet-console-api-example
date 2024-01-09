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
