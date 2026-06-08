using System.Security.Claims;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Services.Interfaces;

namespace TradeOffStackAPI.Services;

/// <summary>Lit l'identité de l'appelant depuis HttpContext.User (claims du JWT validé).</summary>
public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _accessor;

    public CurrentUserService(IHttpContextAccessor accessor) => _accessor = accessor;

    private ClaimsPrincipal? Principal => _accessor.HttpContext?.User;

    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated ?? false;

    public Guid? UserId =>
        Guid.TryParse(Principal?.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;

    public Guid? DepartmentId =>
        Guid.TryParse(Principal?.FindFirstValue(TokenService.DepartmentClaimType), out var id) ? id : null;

    public UserRole? Role =>
        Enum.TryParse<UserRole>(Principal?.FindFirstValue(ClaimTypes.Role), out var role) ? role : null;

    public bool IsInRole(UserRole role) => Role == role;
}
