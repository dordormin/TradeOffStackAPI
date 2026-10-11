using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradeOffStackAPI.Dtos;
using TradeOffStackAPI.Services.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.RateLimiting;

namespace TradeOffStackAPI.Controllers;

/// <summary>
/// Gère l'authentification et l'enregistrement des utilisateurs.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth) => _auth = auth;

    /// <summary>
    /// Enregistre un nouvel utilisateur et génère un jeton d'accès initial.
    /// </summary>
    [HttpPost("register")]
    [AllowAnonymous]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        try
        {
            var result = await _auth.RegisterAsync(request, ct);
            return CreatedAtAction(nameof(Register), new { id = result.UserId }, result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Authentifie un utilisateur et retourne un jeton JWT.
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var result = await _auth.LoginAsync(request, ct);
        if (result is null)
            return Unauthorized(new { message = "Invalid credentials." });

        // F-05: also issue the JWT as an HttpOnly cookie so browsers no longer
        // need localStorage. The token stays in the response body as a fallback
        // for non-browser clients (and plain-HTTP dev, where Secure cookies are refused).
        Response.Cookies.Append("tos_auth", result.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = result.ExpiresAt,
            Path = "/"
        });
        return Ok(result);
    }

    /// <summary>
    /// Logs out the current user by clearing the authentication cookie.
    /// </summary>
    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("tos_auth");
        return Ok(new { message = "Logged out." });
    }
    
    /// <summary>
    /// Vérifie si l'utilisateur est authentifié et renvoie ses informations.
    /// Utile pour initialiser l'état côté client (ex: React).
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    public IActionResult GetMe()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        var email = User.FindFirstValue(ClaimTypes.Email) ?? User.FindFirstValue("email");
        var role = User.FindFirstValue(ClaimTypes.Role) ?? User.FindFirstValue("role");
        var departmentId = User.FindFirstValue("department_id");

        return Ok(new
        {
            UserId = userId,
            Email = email,
            Role = role,
            DepartmentId = departmentId
        });
    }
}