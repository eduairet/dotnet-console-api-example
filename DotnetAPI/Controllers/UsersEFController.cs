using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Data;
using DotnetAPI.Models;
using DotnetAPI.Dtos;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersEFController(IUserRepository userRepository) : ControllerBase
{
    private readonly IUserRepository _repo = userRepository;

    [HttpGet()]
    public IEnumerable<User> GetUsers() => _repo.GetUsers();

    [HttpGet("{userId}")] // Parameters can be added this way
    public User GetUser(int userId) => _repo.GetUser(userId);

    [HttpPost()]
    public IActionResult AddUser(UserAddDto user) => _repo.AddUser(user) ? Ok() : BadRequest("Could not add user");

    [HttpPut()]
    public IActionResult EditUser(User user) => _repo.EditUser(user) ? Ok() : BadRequest("Could not edit user");

    [HttpDelete("{userId}")]
    public IActionResult DeleteUser(int userId) => _repo.DeleteUser(userId) ? Ok() : BadRequest("Could not delete user");
}