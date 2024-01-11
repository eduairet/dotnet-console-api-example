USE DotNetCourseDatabase
GO

EXEC TutorialAppSchema.spPosts_Upsert @Id = 8, @Title = 'First SP Post Updated', @Content = 'This is the first sp post', @UserId = 1
GO

SELECT *
FROM TutorialAppSchema.Posts