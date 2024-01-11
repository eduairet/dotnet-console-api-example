USE DotNetCourseDatabase
GO

CREATE OR ALTER PROCEDURE TutorialAppSchema.spAuth_Get
    @Email NVARCHAR(50),
    @PasswordHash VARBINARY(MAX)
AS
BEGIN
    SELECT Auth.PasswordHash, Auth.PasswordSalt
    FROM TutorialAppSchema.Auth AS Auth
    WHERE Auth.Email = @Email
        AND Auth.PasswordHash = @PasswordHash
END