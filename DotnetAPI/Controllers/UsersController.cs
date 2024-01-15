using Dapper;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Data;
using DotnetAPI.Models;
using DotnetAPI.Dtos;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
// Standardized way to add a route based on the name of the controller
// It takes the string before Controller in the name of the class
public class UsersController(IConfiguration config) : ControllerBase
{
    private readonly DataContext _data = new(config);

    [HttpGet()] // Route path inside the parenthesis
    public IEnumerable<UserComplete> GetUsers(int? userId, bool? isActive)
    {
        string sql = "EXEC TutorialAppSchema.spUsers_Get";
        DynamicParameters sqlParameters = new();
        if (userId != null && userId > 0)
        {
            sql += " @UserId = @UserIDParam,";
            sqlParameters.Add("@UserIDParam", userId, DbType.Int64);
        }
        if (isActive != null)
        {
            sql += " @Active = @ActiveParam,";
            sqlParameters.Add("@ActiveParam", isActive, DbType.Boolean);
        }
        sql = sql.TrimEnd(',');
        IEnumerable<UserComplete> users = _data.LoadDataWithParams<UserComplete>(sql, sqlParameters);
        return users;
    }

    [HttpPut()]
    public IActionResult AddUser(UserUpsertDto user)
    {
        string sql = @"EXEC TutorialAppSchema.spUsers_Upsert
              @FirstName = @FirstNameParam
            , @LastName = @LastNameParam
            , @Email = @EmailParam
            , @Gender = @GenderParam
            , @JobTitle = @JobTitleParam
            , @Department = @DepartmentParam
            , @Salary = @SalaryParam
            , @Active = @ActiveParam";
        DynamicParameters sqlParameters = new();
        sqlParameters.Add("@FirstNameParam", user.FirstName, DbType.String);
        sqlParameters.Add("@LastNameParam", user.LastName, DbType.String);
        sqlParameters.Add("@EmailParam", user.Email, DbType.String);
        sqlParameters.Add("@GenderParam", user.Gender, DbType.String);
        sqlParameters.Add("@JobTitleParam", user.JobTitle, DbType.String);
        sqlParameters.Add("@DepartmentParam", user.Department, DbType.String);
        sqlParameters.Add("@SalaryParam", user.Salary, DbType.Decimal);
        sqlParameters.Add("@ActiveParam", user.Active ? 1 : 0, DbType.Boolean);
        if (user.UserId != null && user.UserId > 0)
        {
            sql += $"\n, @UserId = @UserIDParam";
            sqlParameters.Add("@UserIDParam", user.UserId, DbType.Int64);
        }
        if (_data.ExecuteSqlWithParameters(sql, sqlParameters)) return Ok();
        throw new Exception("Could not add user");
    }

    [HttpDelete("{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        string sql = $"EXEC TutorialAppSchema.spUsers_Delete @UserId = @UserIDParam";
        DynamicParameters sqlParameters = new();
        sqlParameters.Add("@UserIDParam", userId, DbType.Int64);
        if (_data.ExecuteSqlWithParameters(sql, sqlParameters)) return Ok();
        throw new Exception("Could not delete user");
    }
}