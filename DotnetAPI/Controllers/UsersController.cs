using Dapper;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Data;
using DotnetAPI.Models;
using DotnetAPI.Dtos;
using DotnetAPI.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace DotnetAPI.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
// Standardized way to add a route based on the name of the controller
// It takes the string before Controller in the name of the class
public class UsersController(IConfiguration config) : ControllerBase
{
    private readonly DataContext _data = new(config);
    private readonly ReusableSql _reusableSql = new(config);

    [AllowAnonymous]
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
        if (_reusableSql.AddUser(user)) return Ok();
        throw new Exception("Could not add user");
    }

    [HttpDelete()]
    public IActionResult DeleteUser(int userId)
    {
        string sql = $"EXEC TutorialAppSchema.spUsers_Delete @UserId = @UserIDParam";
        DynamicParameters sqlParameters = new();
        sqlParameters.Add("@UserIDParam", userId, DbType.Int64);
        if (_data.ExecuteSqlWithParameters(sql, sqlParameters)) return Ok();
        throw new Exception("Could not delete user");
    }
}