namespace TradeOffStackAPI.Auth;

/// <summary>
/// Paramètres de signature/validation du JWT, liés à la section "Jwt" de la configuration.
/// La clé (Key) NE DOIT PAS être versionnée : utiliser User Secrets / variables d'environnement.
/// </summary>
public sealed class JwtSettings
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public int ExpiryMinutes { get; set; } = 60; // 1 heure par défaut (temporaire)
}