using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Data;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class UsersJobInfoEFController(IConfiguration config) : ControllerBase
{
    private readonly DataContextEF _data = new(config);

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

    [HttpPost("add-user-job-info")]
    public IActionResult AddUserJobInfo(UserJobInfo userJobInfo)
    {
        if (_data.UserJobInfo.Any(u => u.UserId == userJobInfo.UserId))
            return BadRequest("User with the same ID already exists");
        var userJobInfoDb = _mapper.Map<UserJobInfo>(userJobInfo);
        _data.Add(userJobInfoDb);
        if (_data.SaveChanges() > 0) return Ok();
        throw new Exception("Could not add user job info");
    }

    [HttpPut("edit-user-job-info")]
    public IActionResult EditUserJobInfo(UserJobInfo userJobInfo)
    {
        string errMessage = "Could not edit user job info";
        UserJobInfo? userJobInfoDb = _data.UserJobInfo.Where(u => u.UserId == userJobInfo.UserId).FirstOrDefault();
        if (userJobInfoDb != null)
        {
            userJobInfoDb.JobTitle = userJobInfo.JobTitle;
            userJobInfoDb.Department = userJobInfo.Department;
            if (_data.SaveChanges() > 0) return Ok();
            else throw new Exception(errMessage);
        }
        throw new Exception(errMessage);
    }

    [HttpDelete("delete-user-job-info/{userId}")]
    public IActionResult DeleteUserJobInfo(int userId)
    {
        string errMessage = "Could not delete user job info";
        UserJobInfo? userJobInfoDb = _data.UserJobInfo.Where(u => u.UserId == userId).FirstOrDefault();
        if (userJobInfoDb != null)
        {
            _data.UserJobInfo.Remove(userJobInfoDb);
            if (_data.SaveChanges() > 0) return Ok();
            else throw new Exception(errMessage);
        }
        throw new Exception(errMessage);
    }
}