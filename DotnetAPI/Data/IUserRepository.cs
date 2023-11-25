using DotnetAPI.Dtos;
using DotnetAPI.Models;

namespace DotnetAPI.Data;

public interface IUserRepository
{
    bool SaveChanges();
    void AddEntity<T>(T entity);
    void RemoveEntity<T>(T entity);

    #region User
    public IEnumerable<User> GetUsers();
    public User GetUser(int userId);
    public bool AddUser(UserAddDto user);
    public bool EditUser(User user);
    public bool DeleteUser(int userId);
    #endregion

    #region UserJobInfo
    public IEnumerable<UserJobInfo> GetUsersJobInfo();
    public UserJobInfo GetUserJobInfo(int userId);
    public bool AddUserJobInfo(UserJobInfo userJobInfo);
    public bool EditUserJobInfo(UserJobInfo userJobInfo);
    public bool DeleteUserJobInfo(int userId);
    #endregion

    #region UserSalary
    public IEnumerable<UserSalary> GetUsersSalary();
    public UserSalary GetUserSalary(int userId);
    public bool AddUserSalary(UserSalary userSalary);
    public bool EditUserSalary(UserSalary userSalary);
    public bool DeleteUserSalary(int userId);
    #endregion
}