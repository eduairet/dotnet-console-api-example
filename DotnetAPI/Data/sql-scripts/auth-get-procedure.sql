USE DotNetCourseDatabase
GO

CREATE OR ALTER PROCEDURE TutorialAppSchema.spAuth_Get
    @Email NVARCHAR(50)
AS
BEGIN
    SELECT Auth.PasswordHash, Auth.PasswordSalt
    FROM TutorialAppSchema.Auth AS Auth
    WHERE Auth.Email = @Email
END