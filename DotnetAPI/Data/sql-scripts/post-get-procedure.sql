USE DotNetCourseDatabase
GO

CREATE OR ALTER PROCEDURE TutorialAppSchema.spPosts_Get
  /* EXEC TutorialAppSchema.spPosts_Get @UserId = 1, @SearchValue = 'test' */
  /* EXEC TutorialAppSchema.spPosts_Get @PostId = 1 */
  @UserId INT = NULL,
  @PostId INT = NULL,
  @SearchValue NVARCHAR(MAX) = NULL
AS
BEGIN
  SELECT Posts.Id
      , Posts.UserId
      , Posts.Title
      , Posts.Content
      , Posts.CreatedAt
      , Posts.UpdatedAt
  FROM TutorialAppSchema.Posts AS Posts
  WHERE Posts.UserId = ISNULL(@UserId, Posts.UserId)
    AND Posts.Id = ISNULL(@PostId, Posts.Id)
    AND (@SearchValue IS NULL OR
    Posts.Title LIKE '%' + @SearchValue + '%' OR
    Posts.Content LIKE '%' + @SearchValue + '%')
END