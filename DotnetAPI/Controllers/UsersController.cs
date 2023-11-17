using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Data;
using DotnetAPI.Models;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
// Standardized way to add a route based on the name of the controller
// It takes the string before Controller in the name of the class
public class UsersController(IConfiguration config) : ControllerBase
{
    private readonly DataContext _data = new(config);

    [HttpGet("test-connection")]
    public DateTime TestConnection()
    {
        return _data.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }

    [HttpGet()] // Route path inside the parenthesis
    public IEnumerable<User> GetUsers()
    {
        string sql = @"
            SELECT TOP (10) UserId
                ,FirstName
                ,LastName
                ,Email
                ,Gender
                ,Active
            FROM TutorialAppSchema.Users";
        IEnumerable<User> users = _data.LoadData<User>(sql);
        return users;
    }

    [HttpGet("{userId}")] // Parameters can be added this way
    public User GetUser(int userId)
    {
        string sql = @"
            SELECT UserId
                ,FirstName
                ,LastName
                ,Email
                ,Gender
                ,Active
            FROM TutorialAppSchema.Users
                WHERE UserID = " + userId.ToString();
        User user = _data.LoadDataSingle<User>(sql);
        return user;
    }

    [HttpPost("add-user")]
    public IActionResult AddUser(CreateUser user)
    {
        string sql = @"
            INSERT INTO TutorialAppSchema.Users
                    (FirstName
                    ,LastName
                    ,Email
                    ,Gender
                    ,Active)
                VALUES
                    ('" + user.FirstName + "'" +
                    ",'" + user.LastName + "'" +
                    ",'" + user.Email + "'" +
                    ",'" + user.Gender + "'" +
                    ",'" + user.Active + "')";

        if (_data.ExecuteSql(sql))
        {
            return Ok();
        }
        return BadRequest();
    }

    [HttpPut("edit-user")]
    public IActionResult EditUser(User user)
    {
        string sql = @"
            UPDATE TutorialAppSchema.Users
            SET FirstName = '" + user.FirstName +
                "',LastName = '" + user.LastName +
                "',Email = '" + user.Email +
                "',Gender = '" + user.Gender +
                "',Active = " + (user.Active ? 1 : 0) +
            " WHERE UserId = " + user.UserId.ToString();

        if (_data.ExecuteSql(sql))
        {
            return Ok();
        }
        return BadRequest();
    }

    [HttpPost("populate-db")]
    public ActionResult<string> PopulateDB()
    {
        try
        {
            _data.PopulateAll();
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        return "Populated DB";
    }
}