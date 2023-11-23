using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Data;
using DotnetAPI.Models;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersSalaryController(IConfiguration config) : ControllerBase
{
    private readonly DataContext _data = new(config);

    [HttpGet()]
    public IEnumerable<UserSalary> GetUsersSalary()
    {
        string sql = @"
            SELECT UserId,Salary
            FROM TutorialAppSchema.UserSalary";
        IEnumerable<UserSalary> usersSalary = _data.LoadData<UserSalary>(sql);
        return usersSalary;
    }

    [HttpGet("{userId}")]
    public UserSalary GetUserSalary(int userId)
    {
        string sql = @"
            SELECT UserId,Salary
            FROM TutorialAppSchema.UserSalary
            WHERE UserId = " + userId.ToString();
        UserSalary userSalary = _data.LoadDataSingle<UserSalary>(sql);
        return userSalary;
    }

    [HttpPost()]
    public IActionResult AddUserSalary(UserSalary userSalary)
    {
        string checkSql = @"
            SELECT COUNT(*)
            FROM TutorialAppSchema.UserSalary
            WHERE UserId = " + userSalary.UserId.ToString();
        int count = _data.LoadDataSingle<int>(checkSql);

        if (count > 0)
        {
            return BadRequest("User ID already exists in the database");
        }
        else
        {
            string sql = @"
                INSERT INTO TutorialAppSchema.UserSalary
                (UserId, Salary)
                VALUES
                (" + userSalary.UserId.ToString() + "," + userSalary.Salary.ToString() + ")";
            if (_data.ExecuteSql(sql)) return Ok();
            throw new Exception("Could not add user salary");
        }
    }

    [HttpPut()]
    public IActionResult EditUserSalary(UserSalary userSalary)
    {
        string sql = @"
            UPDATE TutorialAppSchema.UserSalary
            SET Salary = " + userSalary.Salary.ToString() +
            " WHERE UserId = " + userSalary.UserId.ToString();
        if (_data.ExecuteSql(sql)) return Ok();
        throw new Exception("Could not edit user salary");
    }

    [HttpDelete("{userId}")]
    public IActionResult DeleteUserSalary(int userId)
    {
        string sql = @"
            DELETE FROM TutorialAppSchema.UserSalary
            WHERE UserId = " + userId.ToString();
        if (_data.ExecuteSql(sql)) return Ok();
        throw new Exception("Could not delete user salary");
    }
}