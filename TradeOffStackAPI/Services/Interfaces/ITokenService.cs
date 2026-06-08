using TradeOffStackAPI.Models;

namespace TradeOffStackAPI.Services.Interfaces;

public interface ITokenService
{
    /// <summary>Génère un JWT signé pour l'utilisateur et renvoie le jeton + sa date d'expiration (UTC).</summary>
    (string Token, DateTime ExpiresAt) GenerateToken(User user);
}
