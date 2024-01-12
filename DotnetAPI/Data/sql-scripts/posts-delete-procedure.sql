USE DotNetCourseDatabase
GO

CREATE OR ALTER PROCEDURE TutorialAppSchema.spPosts_Delete
    @PostId INT,
    @UserId INT = NULL
AS
BEGIN
    DELETE FROM TutorialAppSchema.Posts
    WHERE Id = @PostId
        AND UserId = @UserId
END