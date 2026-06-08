using System.ComponentModel.DataAnnotations;

namespace TradeOffStackAPI.Dtos;

// NOTE : sur un record positionnel, les attributs de validation doivent cibler le
// PARAMÈTRE (pas la propriété générée), sinon la validation ASP.NET Core les ignore
// et lève une InvalidOperationException. On n'utilise donc PAS le préfixe [property: ...].

/// <summary>Inscription d'un nouvel utilisateur (rôle Employee imposé côté serveur).</summary>
public sealed record RegisterRequest(
    [Required, MaxLength(100)] string FirstName,
    [Required, MaxLength(100)] string LastName,
    [Required, EmailAddress, MaxLength(255)] string Email,
    [Required, MinLength(8), MaxLength(128)] string Password,
    [Phone, MaxLength(20)] string? PhoneNumber = null,
    Guid? DepartmentId = null);

/// <summary>Demande de connexion.</summary>
public sealed record LoginRequest(
    [Required, EmailAddress] string Email,
    [Required] string Password);

/// <summary>Réponse d'authentification : jeton signé + métadonnées non sensibles.</summary>
public sealed record AuthResponse(
    string Token,
    DateTime ExpiresAt,
    Guid UserId,
    string Email,
    string Role);
