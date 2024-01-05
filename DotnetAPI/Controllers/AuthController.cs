using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly DataContext _data;
    private readonly IConfiguration _config;
    private readonly string PasswordKey;
    private readonly string TokenKey;
    public AuthController(IConfiguration config)
    {
        _config = config;
        _data = new(_config);
        PasswordKey = _config["JwtSettings:PasswordKey"]!;
        TokenKey = _config["JwtSettings:TokenKey"]!;
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
                if (_data.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters))
                {
                    string sqlAddUser = @"
                        INSERT INTO TutorialAppSchema.Users
                                (FirstName
                                ,LastName
                                ,Email
                                ,Gender
                                ,Active)
                            VALUES
                                ('" + userForRegistration.FirstName + @"'
                                ,'" + userForRegistration.LastName + @"'
                                ,'" + userForRegistration.Email + @"'
                                ,'" + userForRegistration.Gender + @"'
                                ,1);";
                    if (_data.ExecuteSql(sqlAddUser)) return Ok();
                }
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
            // Get the user id from the database using the email
            string sqlGetUserId = "SELECT UserId FROM TutorialAppSchema.Users WHERE Email = '" + userForLogin.Email + "'";
            int userId = _data.LoadDataSingle<int>(sqlGetUserId);
            // Return a dictionary with the token key and the token value
            return Ok(new Dictionary<string, string> { { "token", CreateToken(userId) } });
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

    private string CreateToken(int userId)
    {
        // Create the claims for the token
        Claim[] claims =
        [
            new("userId", userId.ToString())
        ];

        // Create a key from the token key, we need to convert it to a byte array
        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(TokenKey));

        // Create the credentials for the token using the key and the algorithm to sign it
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256Signature);


        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims), // Claims for the token
            SigningCredentials = credentials, // Credentials for the token
            Expires = DateTime.Now.AddDays(7) // Token expires in 7 days
        };

        // Create the token handler and create the token using the token descriptor
        JwtSecurityTokenHandler tokenHandler = new();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}