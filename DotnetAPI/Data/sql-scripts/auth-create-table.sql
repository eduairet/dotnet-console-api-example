USE DotNetCourseDatabase;
GO

CREATE TABLE TutorialAppSchema.Auth
(
    Email NVARCHAR(50) NOT NULL,
    PasswordHash VARBINARY(MAX) NOT NULL,
    PasswordSalt VARBINARY(MAX) NOT NULL
);