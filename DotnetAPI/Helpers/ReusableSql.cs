using System.Data;
using Dapper;
using DotnetAPI.Data;
using DotnetAPI.Dtos;

namespace DotnetAPI.Helpers;

public class ReusableSql(IConfiguration config)
{
    private readonly DataContext _data = new(config);

    public bool AddUser(UserUpsertDto user)
    {
        string sql = @"EXEC TutorialAppSchema.spUsers_Upsert
              @FirstName = @FirstNameParam
            , @LastName = @LastNameParam
            , @Email = @EmailParam
            , @Gender = @GenderParam
            , @JobTitle = @JobTitleParam
            , @Department = @DepartmentParam
            , @Salary = @SalaryParam
            , @Active = @ActiveParam";
        DynamicParameters sqlParameters = new();
        sqlParameters.Add("@FirstNameParam", user.FirstName, DbType.String);
        sqlParameters.Add("@LastNameParam", user.LastName, DbType.String);
        sqlParameters.Add("@EmailParam", user.Email, DbType.String);
        sqlParameters.Add("@GenderParam", user.Gender, DbType.String);
        sqlParameters.Add("@JobTitleParam", user.JobTitle, DbType.String);
        sqlParameters.Add("@DepartmentParam", user.Department, DbType.String);
        sqlParameters.Add("@SalaryParam", user.Salary, DbType.Decimal);
        sqlParameters.Add("@ActiveParam", user.Active ? 1 : 0, DbType.Boolean);
        if (user.UserId != null && user.UserId > 0)
        {
            sql += $"\n, @UserId = @UserIDParam";
            sqlParameters.Add("@UserIDParam", user.UserId, DbType.Int64);
        }
        return _data.ExecuteSqlWithParameters(sql, sqlParameters);
    }
}