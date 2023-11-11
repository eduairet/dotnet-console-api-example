using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Data;
using DotnetAPI.Models;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
// Standardized way to add a route based on the name of the controller
// It takes the string before Controller in the name of the class
public class UserController : ControllerBase
{
    private readonly DataContext _data;
    public UserController(IConfiguration config)
    {
        _data = new DataContext(config);
    }

    [HttpGet("test-connection")]
    public DateTime TestConnection()
    {
        return _data.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }

    [HttpGet("users")] // Route path inside the parenthesis
    public User[] GetUsers()
    {
        User user1 = new () {
            Name = "user1"
        };
        User user2 = new () {
            Name = "user2"
        };
        return new User[] { user1, user2 };
    }

    [HttpGet("user/{userId}")] // Parameters can be added this way
    public string GetUser(int userId)
    {
        return "user" + userId;
    }
}