using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Data;
using DotnetAPI.Models;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class UsersJobInfoEFController(IConfiguration config, IUserRepository userRepository) : ControllerBase
{
    private readonly DataContextEF _data = new(config);
    private readonly IUserRepository _repo = userRepository;
    private readonly IMapper _mapper = new MapperConfiguration(cfg =>
    {
        cfg.CreateMap<UserJobInfo, UserJobInfo>();
    }).CreateMapper();

    [HttpGet()]
    public IEnumerable<UserJobInfo> GetUsersJobInfo()
    {
        IEnumerable<UserJobInfo> usersJobInfo = _data.UserJobInfo;
        if (usersJobInfo != null) return usersJobInfo;
        throw new Exception("Users job info not found");
    }

    [HttpGet("{userId}")]
    public UserJobInfo GetUserJobInfo(int userId)
    {
        UserJobInfo? userJobInfo = _data.UserJobInfo.Where(u => u.UserId == userId).FirstOrDefault();
        if (userJobInfo != null) return userJobInfo;
        throw new Exception("User job info not found");
    }

    [HttpPost()]
    public IActionResult AddUserJobInfo(UserJobInfo userJobInfo)
    {
        if (_data.UserJobInfo.Any(u => u.UserId == userJobInfo.UserId))
            return BadRequest("User with the same ID already exists");
        _repo.AddEntity(userJobInfo);
        if (_repo.SaveChanges()) return Ok();
        throw new Exception("Could not add user job info");
    }

    [HttpPut()]
    public IActionResult EditUserJobInfo(UserJobInfo userJobInfo)
    {
        string errMessage = "Could not edit user job info";
        UserJobInfo? userJobInfoDb = _data.UserJobInfo.Where(u => u.UserId == userJobInfo.UserId).FirstOrDefault();
        if (userJobInfoDb != null)
        {
            _mapper.Map(userJobInfo, userJobInfoDb);
            if (_repo.SaveChanges()) return Ok();
            else return BadRequest(errMessage);
        }
        throw new Exception(errMessage);
    }

    [HttpDelete("{userId}")]
    public IActionResult DeleteUserJobInfo(int userId)
    {
        string errMessage = "Could not delete user job info";
        UserJobInfo? userJobInfoDb = _data.UserJobInfo.Where(u => u.UserId == userId).FirstOrDefault();
        if (userJobInfoDb != null)
        {
            _repo.RemoveEntity(userJobInfoDb);
            if (_repo.SaveChanges()) return Ok();
            else return BadRequest(errMessage);
        }
        throw new Exception(errMessage);
    }
}