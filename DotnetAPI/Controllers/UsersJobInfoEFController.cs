using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Data;
using DotnetAPI.Models;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersJobInfoEFController(IUserRepository userRepository) : ControllerBase
{
    private readonly IUserRepository _repo = userRepository;

    [HttpGet()]
    public IEnumerable<UserJobInfo> GetUsersJobInfo() => _repo.GetUsersJobInfo();

    [HttpGet("{userId}")]
    public UserJobInfo GetUserJobInfo(int userId) => _repo.GetUserJobInfo(userId);

    [HttpPost()]
    public IActionResult AddUserJobInfo(UserJobInfo userJobInfo) => _repo.AddUserJobInfo(userJobInfo) ? Ok() : BadRequest("Could not add user job info");

    [HttpPut()]
    public IActionResult EditUserJobInfo(UserJobInfo userJobInfo) => _repo.EditUserJobInfo(userJobInfo) ? Ok() : BadRequest("Could not edit user job info");

    [HttpDelete("{userId}")]
    public IActionResult DeleteUserJobInfo(int userId) => _repo.DeleteUserJobInfo(userId) ? Ok() : BadRequest("Could not delete user job info");
}