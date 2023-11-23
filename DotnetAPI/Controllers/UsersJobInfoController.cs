using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Data;
using DotnetAPI.Models;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersJobInfoController(IConfiguration config) : ControllerBase
{
    private readonly DataContext _data = new(config);

    [HttpGet()]
    public IEnumerable<UserJobInfo> GetUsersJobInfo()
    {
        string sql = @"
            SELECT UserId,JobTitle,Department
            FROM TutorialAppSchema.UserJobInfo";
        IEnumerable<UserJobInfo> usersJobInfo = _data.LoadData<UserJobInfo>(sql);
        return usersJobInfo;
    }

    [HttpGet("{userId}")]
    public UserJobInfo GetUserJobInfo(int userId)
    {
        string sql = @"
            SELECT UserId,JobTitle,Department
            FROM TutorialAppSchema.UserJobInfo
            WHERE UserId = " + userId.ToString();
        UserJobInfo userJobInfo = _data.LoadDataSingle<UserJobInfo>(sql);
        return userJobInfo;
    }

    [HttpPost()]
    public IActionResult AddUserJobInfo(UserJobInfo userJobInfo)
    {
        string checkSql = @"
            SELECT COUNT(*)
            FROM TutorialAppSchema.UserJobInfo
            WHERE UserId = " + userJobInfo.UserId.ToString();
        int count = _data.LoadDataSingle<int>(checkSql);

        if (count > 0)
        {
            return BadRequest("User ID already exists in the database");
        }
        else
        {
            string sql = @"
                INSERT INTO TutorialAppSchema.UserJobInfo
                (UserId, JobTitle, Department)
                VALUES
                (" + userJobInfo.UserId.ToString() + ",'" + userJobInfo.JobTitle + "','" + userJobInfo.Department + "')";
            if (_data.ExecuteSql(sql)) return Ok();
            throw new Exception("Could not add user job info");
        }
    }

    [HttpPut()]
    public IActionResult EditUserJobInfo(UserJobInfo userJobInfo)
    {
        string sql = @"
            UPDATE TutorialAppSchema.UserJobInfo
            SET JobTitle = '" + userJobInfo.JobTitle + @"'
            ,Department = '" + userJobInfo.Department + @"'
            WHERE UserId = " + userJobInfo.UserId.ToString();
        if (_data.ExecuteSql(sql)) return Ok();
        throw new Exception("Could not edit user job info");
    }

    [HttpDelete("{userId}")]
    public IActionResult DeleteUserJobInfo(int userId)
    {
        string sql = @"
            DELETE FROM TutorialAppSchema.UserJobInfo
            WHERE UserId = " + userId.ToString();
        if (_data.ExecuteSql(sql)) return Ok();
        throw new Exception("Could not delete user job info");
    }
}