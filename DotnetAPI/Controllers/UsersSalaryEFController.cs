using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Data;
using DotnetAPI.Models;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersSalaryEFController(IConfiguration config, IUserRepository userRepository) : ControllerBase
{
    private readonly DataContextEF _data = new(config);
    private readonly IUserRepository _repo = userRepository;
    private readonly IMapper _mapper = new MapperConfiguration(cfg =>
    {
        cfg.CreateMap<UserSalary, UserSalary>();
    }).CreateMapper();

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
        _repo.AddEntity(userSalary);
        if (_repo.SaveChanges()) return Ok();
        throw new Exception("Could not add user salary");
    }

    [HttpPut()]
    public IActionResult EditUserSalary(UserSalary userSalary)
    {
        string errMessage = "Could not edit user salary";
        UserSalary? userSalaryDb = _data.UserSalary.Where(u => u.UserId == userSalary.UserId).FirstOrDefault();
        if (userSalaryDb != null)
        {
            _mapper.Map(userSalary, userSalaryDb);
            if (_repo.SaveChanges()) return Ok();
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
           _repo.RemoveEntity(userSalaryDb);
            if (_repo.SaveChanges()) return Ok();
            else return BadRequest(errMessage);
        }
        throw new Exception(errMessage);
    }
}