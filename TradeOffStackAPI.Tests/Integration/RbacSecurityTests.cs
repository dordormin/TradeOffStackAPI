using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TradeOffStackAPI.Dtos;
using TradeOffStackAPI.Models;
using Xunit;

namespace TradeOffStackAPI.Tests.Integration;

public class RbacSecurityTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public RbacSecurityTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Security_EmployeeCannotDeleteEquipment()
    {
        // Arrange
        var client = _factory.CreateClient();
        var email = $"sec.emp_{Guid.NewGuid()}@trade.io";
        
        var registerResponse = await client.PostAsJsonAsync("/api/auth/register", new RegisterRequest("Sec", "Emp", email, "Password123!"));
        Assert.True(registerResponse.IsSuccessStatusCode, "L'inscription de l'employé a échoué.");
        var authResponse = await registerResponse.Content.ReadFromJsonAsync<AuthResponse>();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResponse!.Token);

        // Act
        var response = await client.DeleteAsync($"/api/equipment/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
    
    [Fact]
    public async Task Security_UnauthenticatedUserCannotReadData()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/equipment");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Security_SelfRegistrationAlwaysAssignsEmployeeRole()
    {
        // Arrange
        var client = _factory.CreateClient();
        var email = $"hacker_{Guid.NewGuid()}@trade.io"; 
        
        // Act
        var request = new RegisterRequest("Hack", "Er", email, "Password123!");
        var registerResponse = await client.PostAsJsonAsync("/api/auth/register", request);
        Assert.True(registerResponse.IsSuccessStatusCode, "L'inscription du 'hacker' a échoué.");
        
        var authResult = await registerResponse.Content.ReadFromJsonAsync<AuthResponse>();

        // Assert
        Assert.Equal("Employee", authResult!.Role);
    }

    /// <summary>
    /// Vérifie que la faille "admin dans l'email" est bien corrigée.
    /// Un email contenant "admin" ne doit PAS donner le rôle Admin.
    /// </summary>
    [Fact]
    public async Task Security_EmailContainingAdminDoesNotGrantAdminRole()
    {
        // Arrange
        var client = _factory.CreateClient();
        var email = $"admin_exploit_{Guid.NewGuid()}@trade.io"; // Email contenant "admin"

        // Act
        var request = new RegisterRequest("Admin", "Hacker", email, "Password123!");
        var registerResponse = await client.PostAsJsonAsync("/api/auth/register", request);
        Assert.True(registerResponse.IsSuccessStatusCode, "L'inscription a échoué.");

        var authResult = await registerResponse.Content.ReadFromJsonAsync<AuthResponse>();

        // Assert — DOIT être Employee, pas Admin
        Assert.Equal("Employee", authResult!.Role);
    }

    /// <summary>
    /// Vérifie que le endpoint /api/auth/me requiert une authentification.
    /// </summary>
    [Fact]
    public async Task Security_MeEndpointRequiresAuthentication()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/auth/me");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}