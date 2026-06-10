namespace TradeOffStackAPI.Auth;

/// <summary>
/// Constantes de rôles pour [Authorize(Roles = ...)]. Les valeurs DOIVENT correspondre
/// à UserRole.ToString() (claim ClaimTypes.Role émis par le TokenService).
/// </summary>
public static class Roles
{
    public const string Admin = "Admin";
    public const string Manager = "Manager";
    public const string Employee = "Employee";
    public const string Tester = "Tester";

    public const string AdminOrManager = Admin + "," + Manager;
    public const string AdminOrTester = Admin + "," + Tester;
    public const string AdminOrManagerOrTester = Admin + "," + Manager + "," + Tester;
}
