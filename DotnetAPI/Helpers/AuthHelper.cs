using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;

namespace DotnetAPI.Helpers;

public partial class AuthHelper(IConfiguration config)
{
    private readonly IConfiguration _config = config;

    public byte[] CreatePasswordHash(string password, byte[] passwordSalt)
    {
        string passwordSaltPlusString =  _config["AppSettings:PasswordKey"]! + Convert.ToBase64String(passwordSalt);
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
}