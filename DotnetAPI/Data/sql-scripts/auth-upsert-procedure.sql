USE DotNetCourseDatabase
GO

DROP PROCEDURE IF EXISTS TutorialAppSchema.spAuth_RecoverPassword
GO

CREATE OR ALTER PROCEDURE TutorialAppSchema.spAuth_Upsert
    @Email NVARCHAR(50),
    @PasswordHash VARBINARY(MAX) = NULL,
    @PasswordSalt VARBINARY(MAX) = NULL
AS
BEGIN
    IF NOT EXISTS (SELECT *
    FROM TutorialAppSchema.Auth
    WHERE Email = @Email)
    BEGIN
        INSERT INTO TutorialAppSchema.Auth
            (Email, PasswordHash, PasswordSalt)
        VALUES
            (@Email, @PasswordHash, @PasswordSalt)
    END
    ELSE
    BEGIN
        UPDATE TutorialAppSchema.Auth
        SET PasswordHash = @PasswordHash,
            PasswordSalt = @PasswordSalt
        WHERE Email = @Email
    END
END
