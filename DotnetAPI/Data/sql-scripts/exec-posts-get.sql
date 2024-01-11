USE DotNetCourseDatabase
GO

EXEC TutorialAppSchema.spPosts_Get @UserId = 1, @SearchValue = 'test'
GO

EXEC TutorialAppSchema.spPosts_Get @PostId = 1
GO