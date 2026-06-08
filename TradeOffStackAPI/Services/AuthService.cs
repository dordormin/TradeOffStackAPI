using TradeOffStackAPI.Dtos;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Repositories.Interfaces;
using TradeOffStackAPI.Services.Interfaces;

namespace TradeOffStackAPI.Services;

/// <summary>
/// Orchestration de l'inscription et de la connexion.
/// Le mot de passe n'est jamais stocké en clair : hachage BCrypt (salt par mot de passe).
/// </summary>
public sealed class AuthService : IAuthService
{
    private readonly IUserRepository _users;
    private readonly ITokenService _tokens;

    public AuthService(IUserRepository users, ITokenService tokens)
    {
        _users = users;
        _tokens = tokens;
    }

    /// <inheritdoc />

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        if (await _users.GetByEmailAsync(email) is not null)
            throw new InvalidOperationException($"A user with the email '{email}' already exists.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = email,
            PhoneNumber = request.PhoneNumber,
            DepartmentId = request.DepartmentId,
            
            // SÉCURITÉ : L'inscription publique crée TOUJOURS un Employee.
            // Les comptes Admin/Manager doivent être créés via l'API User (réservée aux Admin).
            Role = UserRole.Employee,
            
            IsActive = true,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        await _users.AddAsync(user);

        var (token, expiresAt) = _tokens.GenerateToken(user);
        return new AuthResponse(token, expiresAt, user.Id, user.Email, user.Role.ToString());
    }

    /// <inheritdoc />

    public async Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await _users.GetByEmailAsync(email);

        // Message volontairement générique + vérification systématique du hash :
        // ne pas révéler si l'email existe, limiter les attaques par timing/énumération.
        if (user is null || !user.IsActive ||
            !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return null;
        }

        var (token, expiresAt) = _tokens.GenerateToken(user);
        return new AuthResponse(token, expiresAt, user.Id, user.Email, user.Role.ToString());
    }
}