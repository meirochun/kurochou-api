using Kurochou.App.Interfaces.Service;
using Kurochou.Domain.Entities;
using Kurochou.Infra.Common;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Kurochou.App.Service;

public class TokenService(IOptions<AuthenticationSettings> settings) : ITokenService
{
    private readonly AuthenticationSettings _settings = settings.Value;

    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("userId", user.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Jwt.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
                issuer: _settings.Jwt.Issuer,
                audience: _settings.Jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_settings.Jwt.ExpirationMinutes),
                signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}