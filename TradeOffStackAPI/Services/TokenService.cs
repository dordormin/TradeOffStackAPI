using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TradeOffStackAPI.Auth;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Services.Interfaces;

namespace TradeOffStackAPI.Services;

/// <summary>
/// Émission de JWT HMAC-SHA256. Les claims portent l'identité ET le contexte d'autorisation
/// (rôle + département) afin de rester 100 % stateless : aucune session serveur, l'API
/// reconstruit tout depuis le jeton à chaque requête (compatible load balancing).
/// </summary>
public sealed class TokenService : ITokenService
{
    public const string DepartmentClaimType = "department_id";

    private readonly JwtSettings _settings;

    public TokenService(IOptions<JwtSettings> settings) => _settings = settings.Value;

    public (string Token, DateTime ExpiresAt) GenerateToken(User user)
    {
        var expiresAt = DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            // ClaimTypes.Role => exploitable directement par [Authorize(Roles = "...")].
            new(ClaimTypes.Role, user.Role.ToString())
        };

        if (user.DepartmentId.HasValue)
            claims.Add(new Claim(DepartmentClaimType, user.DepartmentId.Value.ToString()));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }
}
