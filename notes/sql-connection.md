# MS SQL Database Connection

## Create a database to work with

-   We can create a Database on MS SQL Server Manager via the UI or by using a script like this one

    ```SQL
    -- Create Database example
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
    ```

    -   You'll need to drop the table if the DB exists (be careful)
        ```SQL
        USE master -- Master database
        DROP DATABASE DotNetCourseDatabase
        ```

## Connect with Azure Studio

-   Open Azure Studio
-   Click on `New` and select `New connection`
-   And fill the fields according to this image (Windows)

    ![Create connection](./images/Screenshot%202023-10-24%20202806.png)

    -   This will allows us to connect with our local SQL server and select our previously created database, alternatively we can open a new query window on Azure Studio and create the database from here

-   Additionally we'll need to install four packages to our project to stablish the connection with the database server

    ```SHELL
    dotnet add package Dapper
    dotnet add package microsoft.data.sqlclient
    dotnet add package microsoft.entityframeworkcore
    dotnet add package microsoft.entityframeworkcore.sqlserver
    ```

-   After running the commands these packages will be installed in our project

    ```XML
    <Project Sdk="Microsoft.NET.Sdk">

    <PropertyGroup>
        <OutputType>Exe</OutputType>
        <TargetFramework>net8.0</TargetFramework>
        <ImplicitUsings>enable</ImplicitUsings>
        <Nullable>enable</Nullable>
    </PropertyGroup>

    <ItemGroup>
        <PackageReference Include="Dapper" Version="2.1.15" />
        <PackageReference Include="microsoft.data.sqlclient" Version="5.1.1" />
        <PackageReference Include="microsoft.entityframeworkcore" Version="7.0.13" />
        <PackageReference Include="microsoft.entityframeworkcore.sqlserver" Version="7.0.13" />
    </ItemGroup>

    </Project>
    ```

## Querying the database

-   First we need to setup the connection on our `Program.cs` file
    ```CSHARP
    // Connection string options
    var connectionOptions = new List<string>()
    {
        "Server=localhost;",
        "Database=DotNetCourseDatabase;",
        "TrustServerCertificate=true;",
        "Trusted_Connection=true;" // Windows Authentication
    };
    string connectionString = string.Join("", connectionOptions);
    // Server=localhost;Database=DotNetCourseDatabase;TrustServerCertificate=true;Trusted_Connection=true;
    ```
-   Then connect the program to the database
    ```CSHARP
    using System.Data;
    using Microsoft.Data.SqlClient;
    // ...
    IDbConnection dbConnection = new SqlConnection(connectionString);
    ```
-   Now you can start querying your database

## Dapper

-   Allows to handle sql queries in a more easier way

    -   Dapper `.QuerySingle<T>()` method
        ```CSHARP
        string sqlCommand = "SELECT GETDATE()";
        DateTime rightNow = dbConnection.QuerySingle<DateTime>(sqlCommand);
        Console.WriteLine(rightNow); // Today's date
        ```
    -   Dapper `.Query<T>()` method
        ```CSHARP
        string sqlQuerySelect = @"
            SELECT _user.Username,
                    _user.FullName,
                    _user.IsActive
                FROM UserSchema._user";
        IEnumerable<User> results = dbConnection.Query<User>(sqlQuerySelect);
        ```
    -   It's a common pattern to create a Data file on our project and mount the sql connection and dapper methods there

        ```CSHARP
        using System.Data;
        using Dapper;
        using Microsoft.Data.SqlClient;

        namespace HelloWorld.Data
        {
            public class DataContext
            {
                private string _connectionString = string.Join("", new List<string>() {
                        "Server=localhost;",
                        "Database=DotNetCourseDatabase;",
                        "TrustServerCertificate=true;",
                        "Trusted_Connection=true;" // Windows Authentication
                });

                // <T> stands for generic type
                public IEnumerable<T> LoadData<T>(string sql)
                {
                    IDbConnection dbConnection = new SqlConnection(_connectionString);
                    return dbConnection.Query<T>(sql);
                }

                public T LoadDataSingle<T>(string sql)
                {
                    IDbConnection dbConnection = new SqlConnection(_connectionString);
                    return dbConnection.QuerySingle<T>(sql);
                }

                public bool ExecuteSql(string sql)
                {
                    IDbConnection dbConnection = new SqlConnection(_connectionString);
                    return dbConnection.Execute(sql) > 0;
                }

                public int ExecuteSqlWithRowCount(string sql)
                {
                    IDbConnection dbConnection = new SqlConnection(_connectionString);
                    return dbConnection.Execute(sql);
                }
            }
        }
        ```

    -   This way we can fetch our data in an easier way
        ```CSHARP
        DataContext dataContext = new();
        string sqlCommand = "SELECT GETDATE()";
        DateTime rightNow = dataContext.LoadDataSingle<DateTime>(sqlCommand);
        Console.WriteLine(rightNow);
        ```
