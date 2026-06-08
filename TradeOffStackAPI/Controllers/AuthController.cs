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
        return result is null
            ? Unauthorized(new { message = "Invalid credentials." })
            : Ok(result);
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