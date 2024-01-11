USE DotNetCourseDatabase
GO

CREATE OR ALTER PROCEDURE TutorialAppSchema.spPosts_Upsert
    /* EXEC TutorialAppSchema.spPosts_Upsert @UserId = 1, @Title = 'Test', @Content = 'Test Content' */
    /* EXEC TutorialAppSchema.spPosts_Upsert @UserId = 1, @Title = 'Test', @Content = 'Test Content' @Id = 1 */
    @UserId INT,
    @Title NVARCHAR(255),
    @Content NVARCHAR(MAX),
    @Id INT = NULL
AS
BEGIN
    IF NOT EXISTS (SELECT *
    FROM TutorialAppSchema.Posts
    WHERE Id = @Id)
        BEGIN
        INSERT INTO TutorialAppSchema.Posts
            (
            UserId,
            Title,
            Content,
            CreatedAt,
            UpdatedAt
            )
        VALUES
            (
                @UserId,
                @Title,
                @Content,
                GETDATE(),
                GETDATE()
            )
    END
    ELSE
        BEGIN
        UPDATE TutorialAppSchema.Posts
            SET
                Title = @Title,
                Content = @Content,
                UpdatedAt = GETDATE()
            WHERE Id = @Id
            AND UserId = @UserId
    END
END