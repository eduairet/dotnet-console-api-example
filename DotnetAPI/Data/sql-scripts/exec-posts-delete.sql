USE DotNetCourseDatabase
GO

EXEC TutorialAppSchema.spPosts_Delete @Id = 5, @UserId = 4
GO

SELECT *
FROM TutorialAppSchema.Posts