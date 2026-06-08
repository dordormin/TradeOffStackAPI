using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;
using TradeOffStackAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// DÉSACTIVER LE REMAPPAGE MICROSOFT DES CLAIMS JWT
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// --- Composition de la DI ---
builder.Services
    .AddPersistence(builder.Configuration)
    .AddApplicationServices(builder.Configuration)
    .AddJwtAuthentication(builder.Configuration)
    .AddScalabilityServices(builder.Configuration)
    .AddHealthChecksConfiguration()
    .AddApiRateLimiting(builder.Environment)
    .AddSwaggerConfiguration();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

var app = builder.Build();

// --- Migration automatique de la base de données au démarrage ---
await app.ApplyDatabaseMigrationsAsync();

// --- Pipeline HTTP ---
app.UseForwardedHeaders();
app.UseExceptionHandling();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TradeOffStack API V1");
    });
}

app.UseHttpsRedirection();

// Rate Limiting (avant l'authentification pour bloquer les abus en amont)
if (!app.Environment.IsEnvironment("Testing"))
{
    app.UseRateLimiter();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapApiHealthChecks();

app.Run();

public partial class Program { }
