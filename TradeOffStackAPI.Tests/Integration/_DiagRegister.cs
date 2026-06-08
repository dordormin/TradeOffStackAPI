using System.Net.Http.Json;
using TradeOffStackAPI.Dtos;
using Xunit;
using Xunit.Abstractions;

namespace TradeOffStackAPI.Tests.Integration;

public class _DiagRegister : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _out;

    public _DiagRegister(CustomWebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _factory = factory;
        _out = output;
    }

    [Fact]
    public async Task Diag_Register()
    {
        var client = _factory.CreateClient();
        var email = $"diag_{Guid.NewGuid()}@trade.io";
        var resp = await client.PostAsJsonAsync("/api/auth/register",
            new RegisterRequest("Diag", "User", email, "Password123!"));
        var body = await resp.Content.ReadAsStringAsync();
        _out.WriteLine($"STATUS={(int)resp.StatusCode}");
        _out.WriteLine($"BODY={body}");
        Assert.True(resp.IsSuccessStatusCode, $"STATUS={(int)resp.StatusCode} BODY={body}");
    }
}
