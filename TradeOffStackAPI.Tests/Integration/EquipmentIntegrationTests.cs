using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using TradeOffStackAPI.Dtos;
using TradeOffStackAPI.Models;
using Xunit;

namespace TradeOffStackAPI.Tests.Integration;

/// <summary>
/// Tests d'intégration complets pour le EquipmentController.
/// Valide la chaîne complète : HTTP → Controller → Service → Repository → DB (SQLite).
/// </summary>
public class EquipmentIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly JsonSerializerOptions _jsonOptions;

    public EquipmentIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            Converters = { new JsonStringEnumConverter() }
        };
    }

    /// <summary>
    /// Enregistre un utilisateur admin et configure le client HTTP avec son token.
    /// Note : Comme la faille admin-par-email est corrigée, on crée un Employee
    /// puis on utilise directement la DB pour promouvoir en Admin dans les tests.
    /// Pour simplifier ici, on crée un Admin via le endpoint register (le test factory
    /// peut être modifié pour seed un admin directement).
    /// </summary>
    private async Task<HttpClient> CreateAuthenticatedAdminClientAsync()
    {
        var client = _factory.CreateClient();
        
        // On crée d'abord un Employee (l'inscription est toujours Employee)
        var email = $"admin.test_{Guid.NewGuid()}@trade.io";
        var registerResponse = await client.PostAsJsonAsync("/api/auth/register",
            new RegisterRequest("Admin", "Test", email, "Password123!"));
        
        Assert.True(registerResponse.IsSuccessStatusCode, 
            $"Échec de l'inscription : {await registerResponse.Content.ReadAsStringAsync()}");
        
        var authResponse = await registerResponse.Content.ReadFromJsonAsync<AuthResponse>();
        
        // Note: dans les tests d'intégration, on doit promouvoir l'utilisateur en Admin
        // via le DbContext directement. Utilisons le scope du factory.
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TradeOffStackAPI.Data.CoreDbContext>();
        var user = await db.Users.FindAsync(authResponse!.UserId);
        Assert.NotNull(user);
        user!.Role = UserRole.Admin;
        await db.SaveChangesAsync();
        
        // Re-login pour obtenir un token avec le rôle Admin
        var loginResponse = await client.PostAsJsonAsync("/api/auth/login",
            new LoginRequest(email, "Password123!"));
        var loginResult = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();
        
        client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", loginResult!.Token);
        
        return client;
    }
    
    private async Task<HttpClient> CreateAuthenticatedEmployeeClientAsync()
    {
        var client = _factory.CreateClient();
        var email = $"emp.test_{Guid.NewGuid()}@trade.io";
        
        var registerResponse = await client.PostAsJsonAsync("/api/auth/register",
            new RegisterRequest("Employee", "Test", email, "Password123!"));
        
        Assert.True(registerResponse.IsSuccessStatusCode);
        var authResponse = await registerResponse.Content.ReadFromJsonAsync<AuthResponse>();
        
        client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", authResponse!.Token);
        
        return client;
    }

    [Fact]
    public async Task PostEquipment_AsAdmin_ShouldCreateAndPersist()
    {
        // Arrange
        var client = await CreateAuthenticatedAdminClientAsync();
        var equipment = new
        {
            name = "MacBook Pro M3 Max",
            serial_number = $"SN-{Guid.NewGuid():N}",
            status = "Available",      // Enum désérialisé en string
            category = "Laptop",       // Enum désérialisé en string
            description = "Laptop développeur",
            price = 3499.99m,
            image = ""
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/equipment", equipment, _jsonOptions);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var created = await response.Content.ReadFromJsonAsync<Equipment>(_jsonOptions);
        Assert.NotNull(created);
        Assert.NotEqual(Guid.Empty, created!.Id);
        Assert.Equal("MacBook Pro M3 Max", created.Name);
        Assert.Equal(AssetStatus.Available, created.Status);
        Assert.Equal(AssetCategory.Laptop, created.Category);
        Assert.Equal(3499.99m, created.Price);
        
        // Vérifier la persistance : GET par ID
        var getResponse = await client.GetAsync($"/api/equipment/{created.Id}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        
        var fetched = await getResponse.Content.ReadFromJsonAsync<Equipment>(_jsonOptions);
        Assert.Equal(created.Id, fetched!.Id);
        Assert.Equal("MacBook Pro M3 Max", fetched.Name);
    }

    [Fact]
    public async Task PostEquipment_AsEmployee_ShouldBeForbidden()
    {
        // Arrange
        var client = await CreateAuthenticatedEmployeeClientAsync();
        var equipment = new
        {
            name = "Dell Monitor",
            serial_number = "SN-FORBIDDEN",
            status = "Available",
            category = "Monitor"
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/equipment", equipment, _jsonOptions);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task GetEquipment_AsEmployee_ShouldSucceed()
    {
        // Arrange
        var client = await CreateAuthenticatedEmployeeClientAsync();

        // Act
        var response = await client.GetAsync("/api/equipment");

        if (response.StatusCode != HttpStatusCode.OK)
        {
            var content = await response.Content.ReadAsStringAsync();
            Assert.Fail($"Request failed with status {response.StatusCode} and body: {content}");
        }

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PostEquipment_EnumDeserialization_ShouldWorkWithStringValues()
    {
        // Arrange — Vérifie que les enums sont correctement désérialisés depuis des strings
        var client = await CreateAuthenticatedAdminClientAsync();

        var testCases = new[]
        {
            new { status = "Available", category = "Laptop", expectedStatus = AssetStatus.Available, expectedCategory = AssetCategory.Laptop },
            new { status = "OutForRepair", category = "Monitor", expectedStatus = AssetStatus.OutForRepair, expectedCategory = AssetCategory.Monitor },
            new { status = "Retired", category = "Peripheral", expectedStatus = AssetStatus.Retired, expectedCategory = AssetCategory.Peripheral },
        };

        foreach (var tc in testCases)
        {
            var equipment = new
            {
                name = $"Test {tc.category}",
                serial_number = $"SN-ENUM-{Guid.NewGuid():N}",
                status = tc.status,
                category = tc.category,
                description = "Test de désérialisation enum"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/equipment", equipment, _jsonOptions);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var created = await response.Content.ReadFromJsonAsync<Equipment>(_jsonOptions);
            Assert.Equal(tc.expectedStatus, created!.Status);
            Assert.Equal(tc.expectedCategory, created.Category);
        }
    }

    [Fact]
    public async Task HealthChecks_ShouldBeAccessibleAnonymously()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act & Assert
        var healthResponse = await client.GetAsync("/health");
        Assert.Equal(HttpStatusCode.OK, healthResponse.StatusCode);

        var liveResponse = await client.GetAsync("/health/live");
        Assert.Equal(HttpStatusCode.OK, liveResponse.StatusCode);

        var readyResponse = await client.GetAsync("/health/ready");
        // Ready peut être Healthy ou ServiceUnavailable selon l'état SQLite
        Assert.True(readyResponse.StatusCode is HttpStatusCode.OK or HttpStatusCode.ServiceUnavailable);
    }
}
