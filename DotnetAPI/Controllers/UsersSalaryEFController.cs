using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Data;
using DotnetAPI.Models;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersSalaryEFController(IUserRepository userRepository) : ControllerBase
{
    private readonly IUserRepository _repo = userRepository;

    [HttpGet()]
    public IEnumerable<UserSalary> GetUsersSalary() => _repo.GetUsersSalary();

    [HttpGet("{userId}")]
    public UserSalary GetUserSalary(int userId) => _repo.GetUserSalary(userId);

    [HttpPost()]
    public IActionResult AddUserSalary(UserSalary userSalary) => _repo.AddUserSalary(userSalary) ? Ok() : BadRequest("Could not add user salary");

    [HttpPut()]
    public IActionResult EditUserSalary(UserSalary userSalary) => _repo.EditUserSalary(userSalary) ? Ok() : BadRequest("Could not edit user salary");

    [HttpDelete("{userId}")]
    public IActionResult DeleteUserSalary(int userId) => _repo.DeleteUserSalary(userId) ? Ok() : BadRequest("Could not delete user salary");
}