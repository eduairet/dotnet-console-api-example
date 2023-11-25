using AutoMapper;
using DotnetAPI.Models;
using DotnetAPI.Dtos;

namespace DotnetAPI.Data;

public class UserRepository(IConfiguration config) : IUserRepository
{
    #region Config

    private readonly string _userNotFound = "User not found";
    private readonly string _jobInfoNotFound = "User job info not found";
    private readonly string _salaryNotFound = "User salary not found";
    private readonly DataContextEF _data = new(config);
    private readonly IMapper _mapper = new MapperConfiguration(cfg =>
    {
        cfg.CreateMap<UserAddDto, User>();
        cfg.CreateMap<User, User>();
    }).CreateMapper();

    #endregion

    #region Helpers

    public bool SaveChanges() => _data.SaveChanges() > 0;

    public void AddEntity<T>(T entity)
    {
        if (entity != null) _data.Add(entity);
    }

    public void RemoveEntity<T>(T entity)
    {
        if (entity != null) _data.Remove(entity);
    }

    #endregion

    #region User

    public IEnumerable<User> GetUsers()
    {
        IEnumerable<User> users = _data.Users;
        if (users != null) return users;
        throw new Exception("There was an error getting the users");
    }

    public User GetUser(int userId)
    {
        User? user = _data.Users.Where(u => u.UserId == userId).FirstOrDefault();
        if (user != null) return user;
        throw new Exception(_userNotFound);
    }

    public bool AddUser(UserAddDto user)
    {
        var userDb = _mapper.Map<User>(user);
        this.AddEntity(userDb);
        return this.SaveChanges();
        throw new Exception("Could not add user");
    }

    public bool EditUser(User user)
    {
        User? userDb = _data.Users.Where(u => u.UserId == user.UserId).FirstOrDefault();
        if (userDb != null)
        {
            _mapper.Map(user, userDb);
            return this.SaveChanges();
        }
        throw new Exception(_userNotFound);
    }

    public bool DeleteUser(int userId)
    {
        User? userDb = _data.Users.Where(u => u.UserId == userId).FirstOrDefault();
        if (userDb != null)
        {
            this.RemoveEntity<User>(userDb);
            return this.SaveChanges();
        }
        throw new Exception(_userNotFound);
    }

    #endregion

    #region UserJobInfo

    public IEnumerable<UserJobInfo> GetUsersJobInfo()
    {
        IEnumerable<UserJobInfo> usersJobInfo = _data.UserJobInfo;
        if (usersJobInfo != null) return usersJobInfo;
        throw new Exception("There was an error getting the users job info");
    }

    public UserJobInfo GetUserJobInfo(int userId)
    {
        UserJobInfo? userJobInfo = _data.UserJobInfo.Where(u => u.UserId == userId).FirstOrDefault();
        if (userJobInfo != null) return userJobInfo;
        throw new Exception(_jobInfoNotFound);
    }

    public bool AddUserJobInfo(UserJobInfo userJobInfo)
    {
        if (_data.UserJobInfo.Any(u => u.UserId == userJobInfo.UserId))
            throw new Exception("User with the same ID already exists");
        this.AddEntity(userJobInfo);
        return this.SaveChanges();
        throw new Exception("Could not add user job info");
    }

    public bool EditUserJobInfo(UserJobInfo userJobInfo)
    {
        UserJobInfo? userJobInfoDb = _data.UserJobInfo.Where(u => u.UserId == userJobInfo.UserId).FirstOrDefault();
        if (userJobInfoDb != null)
        {
            _mapper.Map(userJobInfo, userJobInfoDb);
            return this.SaveChanges();
        }
        throw new Exception(_jobInfoNotFound);
    }

    public bool DeleteUserJobInfo(int userId)
    {
        UserJobInfo? userJobInfoDb = _data.UserJobInfo.Where(u => u.UserId == userId).FirstOrDefault();
        if (userJobInfoDb != null)
        {
            this.RemoveEntity(userJobInfoDb);
            return this.SaveChanges();
        }
        throw new Exception(_jobInfoNotFound);
    }

    #endregion

    #region UserSalary

    public IEnumerable<UserSalary> GetUsersSalary()
    {
        IEnumerable<UserSalary> usersSalary = _data.UserSalary;
        if (usersSalary != null) return usersSalary;
        throw new Exception("There was an error getting the users salary");
    }

    public UserSalary GetUserSalary(int userId)
    {
        UserSalary? userSalary = _data.UserSalary.Where(u => u.UserId == userId).FirstOrDefault();
        if (userSalary != null) return userSalary;
        throw new Exception(_salaryNotFound);
    }

    public bool AddUserSalary(UserSalary userSalary)
    {
        if (_data.UserSalary.Any(u => u.UserId == userSalary.UserId))
            throw new Exception("User with the same ID already exists");
        this.AddEntity(userSalary);
        return this.SaveChanges();
        throw new Exception("Could not add user salary");
    }

    public bool EditUserSalary(UserSalary userSalary)
    {
        UserSalary? userSalaryDb = _data.UserSalary.Where(u => u.UserId == userSalary.UserId).FirstOrDefault();
        if (userSalaryDb != null)
        {
            _mapper.Map(userSalary, userSalaryDb);
            return this.SaveChanges();
        }
        throw new Exception(_salaryNotFound);
    }

    public bool DeleteUserSalary(int userId)
    {
        UserSalary? userSalaryDb = _data.UserSalary.Where(u => u.UserId == userId).FirstOrDefault();
        if (userSalaryDb != null)
        {
            this.RemoveEntity(userSalaryDb);
            return this.SaveChanges();
        }
        throw new Exception(_salaryNotFound);
    }

    #endregion
}