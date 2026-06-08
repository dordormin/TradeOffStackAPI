using System.Net;
using Xunit;

namespace TradeOffStackAPI.Tests.Integration;

public class MetricsAndMonitoringTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public MetricsAndMonitoringTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task HealthCheck_ReturnsHealthy_WhenDatabaseIsAvailable()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/health");
        
        // La réponse contient le statut du service (Healthy)
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Healthy", content);
    }
    
    [Fact]
    public async Task HealthCheckLive_ReturnsHealthy_Instantly()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/health/live");
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}