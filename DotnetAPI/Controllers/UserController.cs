using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Data;
using DotnetAPI.Models;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
// Standardized way to add a route based on the name of the controller
// It takes the string before Controller in the name of the class
public class UserController(IConfiguration config) : ControllerBase
{
    private readonly DataContext _data = new(config);

    [HttpGet("test-connection")]
    public DateTime TestConnection()
    {
        return _data.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }

    [HttpGet("users")] // Route path inside the parenthesis
    public User[] GetUsers()
    {
        return new User[] {
            new() { FirstName = "John", LastName = "Doe" },
            new() { FirstName = "Alice", LastName = "Smith" }
        };
    }

    [HttpGet("user/{userId}")] // Parameters can be added this way
    public string GetUser(int userId)
    {
        return "user" + userId;
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