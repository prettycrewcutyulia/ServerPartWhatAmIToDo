using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ServerPartWhatAmIToDo.Services;

public static class TokenService
{
    public static async Task<string> GenerateToken(string email)
    {
        DotNetEnv.Env.Load();
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRETKEY"));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, email),
            }),
            Expires = null,
            Issuer =  Environment.GetEnvironmentVariable("ValidIssuer"),
            Audience = Environment.GetEnvironmentVariable("ValidAudience"),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);
        
        return tokenString;
    }
}