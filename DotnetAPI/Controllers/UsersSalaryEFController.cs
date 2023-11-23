using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Data;
using DotnetAPI.Models;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersSalaryEFController(IConfiguration config) : ControllerBase
{
    private readonly DataContextEF _data = new(config);

    [HttpGet()]
    public IEnumerable<UserSalary> GetUsersSalary()
    {
        IEnumerable<UserSalary> usersSalary = _data.UserSalary;
        if (usersSalary != null) return usersSalary;
        throw new Exception("Users salary not found");
    }

    [HttpGet("{userId}")]
    public UserSalary GetUserSalary(int userId)
    {
        UserSalary? userSalary = _data.UserSalary.Where(u => u.UserId == userId).FirstOrDefault();
        if (userSalary != null) return userSalary;
        throw new Exception("User salary not found");
    }

    [HttpPost()]
    public IActionResult AddUserSalary(UserSalary userSalary)
    {
        if (_data.UserSalary.Any(u => u.UserId == userSalary.UserId))
            return BadRequest("User with the same ID already exists");
        _data.Add(userSalary);
        if (_data.SaveChanges() > 0) return Ok();
        throw new Exception("Could not add user salary");
    }

    [HttpPut()]
    public IActionResult EditUserSalary(UserSalary userSalary)
    {
        string errMessage = "Could not edit user salary";
        UserSalary? userSalaryDb = _data.UserSalary.Where(u => u.UserId == userSalary.UserId).FirstOrDefault();
        if (userSalaryDb != null)
        {
            userSalaryDb.Salary = userSalary.Salary;
            if (_data.SaveChanges() > 0) return Ok();
            else return BadRequest(errMessage);
        }
        throw new Exception(errMessage);
    }

    [HttpDelete("{userId}")]
    public IActionResult DeleteUserSalary(int userId)
    {
        string errMessage = "Could not delete user salary";
        UserSalary? userSalaryDb = _data.UserSalary.Where(u => u.UserId == userId).FirstOrDefault();
        if (userSalaryDb != null)
        {
            _data.UserSalary.Remove(userSalaryDb);
            if (_data.SaveChanges() > 0) return Ok();
            else return BadRequest(errMessage);
        }
        throw new Exception(errMessage);
    }
}