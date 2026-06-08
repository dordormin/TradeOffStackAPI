using TradeOffStackAPI.Dtos;

namespace TradeOffStackAPI.Services.Interfaces;

public interface IAuthService
{
    /// <summary>Inscrit un utilisateur (rôle Employee) et renvoie un jeton. Lève en cas d'email déjà utilisé.</summary>
    Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default);

    /// <summary>Vérifie les identifiants. Renvoie null si invalides ou compte désactivé.</summary>
    Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken ct = default);
}
