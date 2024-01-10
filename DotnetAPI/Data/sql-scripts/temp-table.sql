USE DotNetCourseDatabase
GO

ALTER PROCEDURE TutorialAppSchema.spUsers_Get
    /* EXEC TutorialAppSchema.spUsers_Get @UserId = 1, @Active = 1 */
    @UserId INT = NULL
    ,
    @Active BIT = NULL
AS
BEGIN
    DROP TABLE IF EXISTS #AvgDeptSalary

    SELECT UserJobInfo.Department
        , AVG(UserSalary.Salary) AS AvgSalary
    INTO #AvgDeptSalary
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
        AND ISNULL(Users.Active, 0) = COALESCE(@Active, Users.Active, 0)
/* We need to wrap the Users.Active value with ISNULL to make it work */
END