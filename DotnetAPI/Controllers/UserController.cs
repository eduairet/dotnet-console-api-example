using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
// Standardized way to add a route based on the name of the controller
// It takes the string before Controller in the name of the class
public class UserController : ControllerBase
{
    [HttpGet("users")] // Route path inside the parenthesis
    public string[] GetUsers()
    {
        return new string[] {"user1", "user2"};
    }

    [HttpGet("user/{userId}")] // Parameters can be added this way
    public string GetUser(int userId)
    {
        return "user" + userId;
    }
}