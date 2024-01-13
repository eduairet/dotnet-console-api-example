USE DotNetCourseDatabase
GO

CREATE OR ALTER PROCEDURE TutorialAppSchema.spAuth_EmailExists
    @Email nvarchar(255)
AS
BEGIN
    SELECT *
    FROM TutorialAppSchema.Auth
    WHERE Email = @Email
END