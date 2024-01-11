USE DotNetCourseDatabase
GO

CREATE OR ALTER PROCEDURE TutorialAppSchema.spPosts_Delete
    @Id INT,
    @UserId INT = NULL
AS
BEGIN
    DELETE FROM TutorialAppSchema.Posts
    WHERE Id = @Id
        AND UserId = @UserId
END