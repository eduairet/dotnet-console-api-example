using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using dotenv.net;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly DataContext _data;
    private readonly IConfiguration _config;
    private readonly string PasswordKey;
    public AuthController(IConfiguration config)
    {
        _config = config;
        _data = new(_config);
        PasswordKey = DotEnv.Read()["PASSWORD_KEY"];
    }

    [HttpPost("register")]
    public IActionResult Register(UserForRegistrationDto userForRegistration)
    {
        if (userForRegistration.Password == userForRegistration.PasswordConfirm)
        {
            string sql = "SELECT Email FROM TutorialAppSchema.Auth WHERE Email = '" + userForRegistration.Email + "'";
            IEnumerable<string> existingUsers = _data.LoadData<string>(sql);
            if (existingUsers?.Count() == 0)
            {
                byte[] passwordSalt = new byte[128 / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetNonZeroBytes(passwordSalt); // Generate a random salt
                }
                byte[] passwordHash = CreatePasswordHash(userForRegistration.Password, passwordSalt);
                string sqlAddAuth = @"
                    INSERT INTO TutorialAppSchema.Auth (
                        Email,
                        PasswordHash,
                        PasswordSalt
                    ) VALUES (
                        '" + userForRegistration.Email + @"',
                        @PasswordHash, @PasswordSalt
                    )";
                List<SqlParameter> sqlParameters = [];
                SqlParameter passwordSaltParameter = new("@PasswordHash", SqlDbType.VarBinary) { Value = passwordHash };
                SqlParameter passwordHashParameter = new("@PasswordSalt", SqlDbType.VarBinary) { Value = passwordSalt };
                sqlParameters.Add(passwordSaltParameter);
                sqlParameters.Add(passwordHashParameter);
                if (_data.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters)) return Ok();
                return BadRequest("Failed to register user");
            }
            return BadRequest("Email already exists");
        }
        return BadRequest("Passwords do not match");
    }

    [HttpPost("login")]
    public IActionResult Login(UserForLoginDto userForLogin)
    {
        string sql = "SELECT PasswordHash, PasswordSalt FROM TutorialAppSchema.Auth WHERE Email = '" + userForLogin.Email + "'";
        var userForConfirmation = _data.LoadDataSingle<UserForLoginConfirmationDto>(sql);
        byte[] passwordHash = CreatePasswordHash(userForLogin.Password, userForConfirmation.PasswordSalt);
        if (passwordHash.SequenceEqual(userForConfirmation.PasswordHash)) // We can't use == to compare byte arrays
        {
            return Ok("Logged in");
        }
        return StatusCode(401, "Incorrect password");
    }

    private byte[] CreatePasswordHash(string password, byte[] passwordSalt)
    {
        string passwordSaltPlusString = PasswordKey + Convert.ToBase64String(passwordSalt);
        byte[] passwordHash = KeyDerivation.Pbkdf2(
            password: password,
            salt: Encoding.ASCII.GetBytes(passwordSaltPlusString), // Create a salted hash of the password
            prf: KeyDerivationPrf.HMACSHA256, // Schema for password hashing
            iterationCount: 10000,
            numBytesRequested: 256 / 8
        );
        return passwordHash;
    }

}