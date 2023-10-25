USE master
DROP DATABASE DotNetCourseDatabase

CREATE DATABASE DotNetCourseDatabase
GO
 
USE DotNetCourseDatabase
GO
 
CREATE SCHEMA UserSchema
GO
 
CREATE TABLE UserSchema._user (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50),
    FullName NVARCHAR(255),
    IsActive BIT,
    UserPassword NVARCHAR(50)
);

SELECT * FROM UserSchema._user;