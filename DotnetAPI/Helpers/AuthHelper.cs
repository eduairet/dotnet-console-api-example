using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using DotnetAPI.Data;

namespace DotnetAPI.Helpers;

public partial class AuthHelper(IConfiguration config)
{
    private readonly IConfiguration _config = config;
    private readonly DataContext _data = new(config);

    public byte[] CreatePasswordHash(string password, byte[] passwordSalt)
    {
        string passwordSaltPlusString = _config["AppSettings:PasswordKey"]! + Convert.ToBase64String(passwordSalt);
        byte[] passwordHash = KeyDerivation.Pbkdf2(
            password: password,
            salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8
        );
        return passwordHash;
    }

    public string CreateToken(int userId)
    {
        Claim[] claims =
        [
            new("userId", userId.ToString())
        ];

        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_config["JwtSettings:TokenKey"]!));
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256Signature);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = credentials,
            Expires = DateTime.Now.AddDays(7)
        };

        JwtSecurityTokenHandler tokenHandler = new();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public bool SetPassword(string email, string password)
    {
        byte[] passwordSalt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetNonZeroBytes(passwordSalt);
        }
        byte[] passwordHash = CreatePasswordHash(password, passwordSalt);
        string sqlAddAuth = @$"EXEC TutorialAppSchema.spAuth_Upsert @Email = @EmailParam,
                    @PasswordHash = @PasswordHashParam,
                    @PasswordSalt = @PasswordSaltParam";
        List<SqlParameter> sqlParameters = [];
        SqlParameter emailParameter = new("@EmailParam", SqlDbType.VarChar) { Value = email };
        sqlParameters.Add(emailParameter);
        SqlParameter passwordSaltParameter = new("@PasswordHashParam", SqlDbType.VarBinary) { Value = passwordHash };
        sqlParameters.Add(passwordSaltParameter);
        SqlParameter passwordHashParameter = new("@PasswordSaltParam", SqlDbType.VarBinary) { Value = passwordSalt };
        sqlParameters.Add(passwordHashParameter);
        return _data.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters);
    }
}