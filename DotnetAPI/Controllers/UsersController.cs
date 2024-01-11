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
        if (userId != null && userId > 0) sql += $" @UserID = {userId},";
        if (isActive != null) sql += $" @Active = {((bool)isActive ? 1 : 0)},";
        sql = sql.TrimEnd(',');
        IEnumerable<UserComplete> users = _data.LoadData<UserComplete>(sql);
        return users;
    }

    [HttpPut()]
    public IActionResult AddUser(UserUpsertDto user)
    {
        string sql = @$"EXEC TutorialAppSchema.spUsers_Upsert @FirstName = '{user.FirstName}'
            , @LastName = '{user.LastName}'
            , @Email = '{user.Email}'
            , @Gender = '{user.Gender}'
            , @JobTitle = '{user.JobTitle}'
            , @Department = '{user.Department}'
            , @Salary = {user.Salary}
            , @Active = {(user.Active ? 1 : 0)}";
        if (user.UserId != null && user.UserId > 0) sql += $"\n, @UserId = {user.UserId}";
        if (_data.ExecuteSql(sql)) return Ok();
        throw new Exception("Could not add user");
    }

    [HttpDelete("{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        string sql = $"EXEC TutorialAppSchema.spUsers_Delete @UserId = {userId}";
        if (_data.ExecuteSql(sql)) return Ok();
        throw new Exception("Could not delete user");
    }
}