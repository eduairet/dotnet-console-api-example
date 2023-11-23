using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Data;
using DotnetAPI.Models;
using DotnetAPI.Dtos;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersEFController(IConfiguration config) : ControllerBase
{
    private readonly DataContextEF _data = new(config);
    private readonly IMapper _mapper = new MapperConfiguration(cfg =>
    {
        cfg.CreateMap<UserAddDto, User>();
    }).CreateMapper();

    [HttpGet()]
    public IEnumerable<User> GetUsers()
    {
        IEnumerable<User> users = _data.Users;
        if (users != null) return users;
        throw new Exception("Users not found");
    }

    [HttpGet("{userId}")] // Parameters can be added this way
    public User GetUser(int userId)
    {
        User? user = _data.Users.Where(u => u.UserId == userId).FirstOrDefault();
        if (user != null) return user;
        throw new Exception("User not found");
    }

    [HttpPost("add-user")]
    public IActionResult AddUser(UserAddDto user)
    {
        var userDb = _mapper.Map<User>(user);
        _data.Add(userDb);
        if (_data.SaveChanges() > 0) return Ok();
        throw new Exception("Could not add user"); ;
    }

    [HttpPut("edit-user")]
    public IActionResult EditUser(User user)
    {
        string errMessage = "Could not edit user";
        User? userDb = _data.Users.Where(u => u.UserId == user.UserId).FirstOrDefault();
        if (userDb != null)
        {
            userDb.Active = user.Active;
            userDb.Gender = user.Gender;
            userDb.Email = user.Email;
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            if (_data.SaveChanges() > 0) return Ok();
            else return BadRequest(errMessage);
        }
        throw new Exception(errMessage);
    }

    [HttpDelete("delete-user/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        string errMessage = "Could not delete user";
        User? userDb = _data.Users.Where(u => u.UserId == userId).FirstOrDefault();
        if (userDb != null)
        {
            _data.Users.Remove(userDb);
            if (_data.SaveChanges() > 0) return Ok();
            else return BadRequest(errMessage);
        }
        throw new Exception(errMessage);
    }
}