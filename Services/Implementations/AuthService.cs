using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Presentes.Services.Interfaces;

namespace Presentes.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string GenerateJwtToken(string email, string role)
    {
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var key = _configuration["Jwt:Key"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim("email", email),
            new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(issuer: issuer, audience: audience, expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials, claims: claims);
        
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var stringToken = tokenHandler.WriteToken(token);
        return stringToken;
    }
}