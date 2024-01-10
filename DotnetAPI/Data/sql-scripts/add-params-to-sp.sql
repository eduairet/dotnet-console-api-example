USE DotNetCourseDatabase
GO

ALTER PROCEDURE TutorialAppSchema.spUsers_Get
    /* EXEC TutorialAppSchema.sp_Get @UserId = 1 */
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