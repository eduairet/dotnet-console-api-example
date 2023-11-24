using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Data;
using DotnetAPI.Models;
using DotnetAPI.Dtos;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersEFController(IConfiguration config, IUserRepository userRepository) : ControllerBase
{
    private readonly DataContextEF _data = new(config);
    private readonly IUserRepository _repo = userRepository;
    private readonly IMapper _mapper = new MapperConfiguration(cfg =>
    {
        cfg.CreateMap<UserAddDto, User>();
        cfg.CreateMap<User, User>();
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

    [HttpPost()]
    public IActionResult AddUser(UserAddDto user)
    {
        var userDb = _mapper.Map<User>(user);
        _repo.AddEntity(userDb);
        if (_repo.SaveChanges()) return Ok();
        throw new Exception("Could not add user"); ;
    }

    [HttpPut()]
    public IActionResult EditUser(User user)
    {
        string errMessage = "Could not edit user";
        User? userDb = _data.Users.Where(u => u.UserId == user.UserId).FirstOrDefault();
        if (userDb != null)
        {
            _mapper.Map(user, userDb);
            if (_repo.SaveChanges()) return Ok();
            else return BadRequest(errMessage);
        }
        throw new Exception(errMessage);
    }

    [HttpDelete("{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        string errMessage = "Could not delete user";
        User? userDb = _data.Users.Where(u => u.UserId == userId).FirstOrDefault();
        if (userDb != null)
        {
            _repo.RemoveEntity<User>(userDb);
            if (_repo.SaveChanges()) return Ok();
            else return BadRequest(errMessage);
        }
        throw new Exception(errMessage);
    }
}