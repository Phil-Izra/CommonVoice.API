namespace CommonVoice.API.Infrastructure.Security;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CommonVoice.API.Application.Common;
using CommonVoice.API.Domain.Aggregates.Users;
using CommonVoice.API.Models.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

public sealed class JwtTokenService(IOptions<JwtSettings> options) : IJwtTokenService
{
    private readonly JwtSettings _settings = options.Value;

    public (string Token, DateTime ExpiresAt) Generate(User user)
    {
        var key         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiresAt   = DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub,   user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role,               user.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString()),
        };

        var token = new JwtSecurityToken(
            issuer:             _settings.Issuer,
            audience:           _settings.Audience,
            claims:             claims,
            expires:            expiresAt,
            signingCredentials: credentials);

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }
}
