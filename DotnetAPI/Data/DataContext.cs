using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using Newtonsoft.Json;
using DotnetAPI.Models;

namespace DotnetAPI.Data;

public class DataContext
{
    private readonly IConfiguration _config;
    private readonly string? _connectionString;

    public DataContext(IConfiguration config)
    {
        _config = config;
        _connectionString = _config.GetConnectionString("DefaultConnection");
    }
    public IEnumerable<T> LoadData<T>(string sql)
    {
        SqlConnection dbConnection = new(_connectionString);
        return dbConnection.Query<T>(sql);
    }
    public T LoadDataSingle<T>(string sql)
    {
        SqlConnection dbConnection = new(_connectionString);
        return dbConnection.QuerySingle<T>(sql);
    }
    public IEnumerable<T> LoadDataWithParams<T>(string sql, List<SqlParameter> sqlParameters)
    {
        SqlConnection dbConnection = new(_connectionString);
        return dbConnection.Query<T>(sql, sqlParameters);
    }
    public T LoadDataSingleWithParams<T>(string sql, DynamicParameters sqlParameters)
    {
        SqlConnection dbConnection = new(_connectionString);
        return dbConnection.QuerySingle<T>(sql, sqlParameters);
    }
    public bool ExecuteSql(string sql)
    {
        SqlConnection dbConnection = new(_connectionString);
        return dbConnection.Execute(sql) > 0;
    }
    public bool ExecuteSqlWithParameters(string sql, DynamicParameters sqlParameters)
    {
        SqlConnection dbConnection = new(_connectionString);
        return dbConnection.Execute(sql, sqlParameters) > 0;
    }
    public int ExecuteSqlRowCount(string sql)
    {
        SqlConnection dbConnection = new(_connectionString);
        return dbConnection.Execute(sql);
    }
    public int ExecuteSqlRowCountWithParams(string sql, DynamicParameters sqlParameters)
    {
        SqlConnection dbConnection = new(_connectionString);
        return dbConnection.Execute(sql, sqlParameters);
    }
    public void DeleteDataBeforePopulate()
    {
        SqlConnection dbConnection = new(_connectionString);

        string deleteUsersData = "DELETE FROM TutorialAppSchema.Users;";
        dbConnection.Execute(deleteUsersData);

        string deleteUserSalaryData = "DELETE FROM TutorialAppSchema.UserSalary;";
        dbConnection.Execute(deleteUserSalaryData);

        string deleteUserJobInfoData = "DELETE FROM TutorialAppSchema.UserJobInfo;";
        dbConnection.Execute(deleteUserJobInfoData);
    }
    public void PopulateUsers()
    {
        string json = File.ReadAllText("Data/data-jsons/Users.json");
        List<User>? users = JsonConvert.DeserializeObject<List<User>>(json);
        SqlConnection dbConnection = new(_connectionString);

        foreach (var user in users!)
        {
            string sql = @"
                INSERT INTO TutorialAppSchema.Users
                        (FirstName
                        ,LastName
                        ,Email
                        ,Gender
                        ,Active)
                    VALUES
                        (@FirstName
                        ,@LastName
                        ,@Email
                        ,@Gender
                        ,@Active);";
            dbConnection.Execute(sql, user);
            dbConnection.Close();
        }
    }
    public void PopulateUserSalary()
    {
        string json = File.ReadAllText("Data/data-jsons/UserSalary.json");
        List<UserSalary>? userSalaries = JsonConvert.DeserializeObject<List<UserSalary>>(json);
        SqlConnection dbConnection = new(_connectionString);

        foreach (var userSalary in userSalaries!)
        {
            string sql = $@"
                INSERT INTO TutorialAppSchema.UserSalary
                        (UserId
                        ,Salary)
                    VALUES
                        (@UserId
                        ,@Salary);";
            dbConnection.Execute(sql, userSalary);
            dbConnection.Close();
        }
    }
    public void PopulateUserJobInfo()
    {
        string json = File.ReadAllText("Data/data-jsons/UserJobInfo.json");
        List<UserJobInfo>? userJobInfos = JsonConvert.DeserializeObject<List<UserJobInfo>>(json);
        SqlConnection dbConnection = new(_connectionString);

        foreach (var userJobInfo in userJobInfos!)
        {
            string sql = $@"
                INSERT INTO TutorialAppSchema.UserJobInfo
                        (UserId
                        ,JobTitle
                        ,Department)
                    VALUES
                        (@UserId
                        ,@JobTitle
                        ,@Department);";
            dbConnection.Execute(sql, userJobInfo);
            dbConnection.Close();
        }
    }
    public void PopulateAll()
    {
        DeleteDataBeforePopulate();
        Console.WriteLine("Deleted");
        PopulateUsers();
        Console.WriteLine("Users Populated");
        PopulateUserSalary();
        PopulateUserJobInfo();
    }
}
