using TradeOffStackAPI.Models;

namespace TradeOffStackAPI.Services.Interfaces;

/// <summary>
/// Expose l'identité de l'appelant reconstruite depuis le JWT de la requête courante.
/// Stateless : aucune donnée serveur, tout provient des claims (compatible load balancing).
/// </summary>
public interface ICurrentUserService
{
    Guid? UserId { get; }
    Guid? DepartmentId { get; }
    UserRole? Role { get; }
    bool IsAuthenticated { get; }
    bool IsInRole(UserRole role);
}
