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