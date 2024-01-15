using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Helpers;

namespace DotnetAPI.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AuthController(IConfiguration config) : ControllerBase
{
    private readonly DataContext _data = new(config);
    private readonly AuthHelper _authHelper = new(config);

    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult Register(UserForRegistrationDto userForRegistration)
    {
        if (userForRegistration.Password == userForRegistration.PasswordConfirm)
        {
            string sql = $"EXEC TutorialAppSchema.spAuth_EmailExists @Email = '{userForRegistration.Email}'";
            IEnumerable<string> existingUsers = _data.LoadData<string>(sql);
            if (existingUsers?.Count() == 0)
            {
                if (_authHelper.SetPassword(userForRegistration.Email, userForRegistration.Password))
                {
                    string sqlAddUser = @$"EXEC TutorialAppSchema.spUsers_Upsert @FirstName = '{userForRegistration.FirstName}'
                                , @LastName = '{userForRegistration.LastName}'
                                , @Email = '{userForRegistration.Email}'
                                , @Gender = '{userForRegistration.Gender}'
                                , @JobTitle = '{userForRegistration.JobTitle}'
                                , @Department = '{userForRegistration.Department}'
                                , @Salary = {userForRegistration.Salary}
                                , @Active = 1";
                    if (_data.ExecuteSql(sqlAddUser)) return Ok();
                }
                return BadRequest("Failed to register user");
            }
            return BadRequest("Email already exists");
        }
        return BadRequest("Passwords do not match");
    }

    [HttpPut("reset-password")]
    public IActionResult ResetPassword(UserForLoginDto userForResetPassword)
    {
        if (_authHelper.SetPassword(userForResetPassword.Email, userForResetPassword.Password))
            return Ok();
        return BadRequest("Failed to reset password");
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login(UserForLoginDto userForLogin)
    {
        var userForConfirmation = _authHelper.Login(userForLogin.Email);
        byte[] passwordHash = _authHelper.CreatePasswordHash(userForLogin.Password, userForConfirmation.PasswordSalt);
        if (passwordHash.SequenceEqual(userForConfirmation.PasswordHash))
        {
            // Get the user id from the database using the email
            string sqlGetUserId = $"SELECT UserId FROM TutorialAppSchema.Users WHERE Email = '{userForLogin.Email}'";
            int userId = _data.LoadDataSingle<int>(sqlGetUserId);
            // Return a dictionary with the token key and the token value
            return Ok(new Dictionary<string, string> { { "token", _authHelper.CreateToken(userId) } });
        }
        return StatusCode(401, "Incorrect password");
    }

    [HttpGet("refresh-token")]
    public string RefreshToken()
    {
        // User ID comes from the token claims
        string sql = $"SELECT UserId FROM TutorialAppSchema.Users WHERE UserId = '{User.FindFirst("userId")?.Value}'";
        int userId = _data.LoadDataSingle<int>(sql);
        return _authHelper.CreateToken(userId);
    }
}