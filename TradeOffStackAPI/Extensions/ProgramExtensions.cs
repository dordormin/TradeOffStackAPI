using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TradeOffStackAPI.Data;
using TradeOffStackAPI.Services.Interfaces;

namespace TradeOffStackAPI.Extensions;

/// <summary>
/// Extension class containing configuration methods for the application startup pipeline (Program.cs).
/// Encapsulates complexity to keep the entry point clean and readable.
/// </summary>
public static class ProgramExtensions
{
    /// <summary>
    /// Configures and adds rate limiting to protect the API against abuse and brute-force attacks.
    /// Manages global per-IP rules and strict rules specific for authentication.
    /// </summary>
    /// <param name="services">The application's service collection.</param>
    /// <param name="environment">The current web environment (used to disable limits in Testing mode).</param>
    /// <returns>The updated service collection for chaining.</returns>
    public static IServiceCollection AddApiRateLimiting(this IServiceCollection services, IWebHostEnvironment environment)
    {
        if (environment.IsEnvironment("Testing"))
        {
            return services;
        }

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            // Global policy: 100 requests / minute per IP
            options.AddFixedWindowLimiter("global", opt =>
            {
                opt.PermitLimit = 100;
                opt.Window = TimeSpan.FromMinutes(1);
                opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                opt.QueueLimit = 0; // No queue: immediate rejection
            });

            // Strict policy for authentication: 10 requests / minute per IP
            options.AddFixedWindowLimiter("auth", opt =>
            {
                opt.PermitLimit = 10;
                opt.Window = TimeSpan.FromMinutes(1);
                opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                opt.QueueLimit = 0;
            });

            // IP partitioning for the default global policy
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 100,
                        Window = TimeSpan.FromMinutes(1)
                    }));
        });

        return services;
    }

    /// <summary>
    /// Configures Swagger/OpenAPI to generate API documentation.
    /// Includes configuration required to support JWT token authentication in the Swagger UI.
    /// </summary>
    /// <param name="services">The application's service collection.</param>
    /// <returns>The updated service collection for chaining.</returns>
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "TradeOffStack API", Version = "v1" });

            // Swagger configuration for JWT
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization.\n\nPaste ONLY your token here (without the Bearer prefix).",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }

    /// <summary>
    /// Automatically applies Entity Framework Core database migrations at application startup.
    /// Ensures the database schema is always up to date (Idempotent). Skipped if using SQLite (Tests).
    /// </summary>
    /// <param name="app">The API's WebApplication instance.</param>
    /// <returns>An asynchronous task representing the migration operation.</returns>
    public static async Task ApplyDatabaseMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        
        try
        {
            var userManager = scope.ServiceProvider.GetRequiredService<IUserService>();
            var coreDb = scope.ServiceProvider.GetRequiredService<CoreDbContext>();
            var assetDb = scope.ServiceProvider.GetRequiredService<AssetDbContext>();

            if (!coreDb.Database.ProviderName?.Contains("Sqlite", StringComparison.OrdinalIgnoreCase) ?? false)
            {
                logger.LogInformation("Applying Core database migrations...");
                await coreDb.Database.MigrateAsync();
                await coreDb.Database.ExecuteSqlRawAsync("ALTER TYPE user_role ADD VALUE IF NOT EXISTS 'Tester';");
                logger.LogInformation("Core database migrations applied successfully.");
            }

            if (!assetDb.Database.ProviderName?.Contains("Sqlite", StringComparison.OrdinalIgnoreCase) ?? false)
            {
                logger.LogInformation("Applying Asset database migrations...");
                await assetDb.Database.MigrateAsync();
                logger.LogInformation("Asset database migrations applied successfully.");
            }
            else
            {
                logger.LogInformation("SQLite provider detected — migration skipped (EnsureCreated is used).");
            }
            
            // ==========================================
            // SÉCURITÉ : Initialisation du Département, Administrateur et Testeur
            // ==========================================
            
            // Ensure the main department exists
            var itDept = await coreDb.Departments.FirstOrDefaultAsync(d => d.Name == "IT Department");
            if (itDept == null)
            {
                itDept = new TradeOffStackAPI.Models.Department
                {
                    Id = Guid.NewGuid(),
                    Name = "IT Department",
                    Description = "Global Information Technology"
                };
                coreDb.Departments.Add(itDept);
                await coreDb.SaveChangesAsync();
            }

            if (!await coreDb.Users.AnyAsync(u => u.Email == "admin@tradeoffstack.com"))
            {
                logger.LogInformation("Creating default Administrator account...");
                var adminUser = new TradeOffStackAPI.Models.User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "System",
                    LastName = "Admin",
                    Email = "admin@tradeoffstack.com",
                    Role = TradeOffStackAPI.Models.UserRole.Admin,
                    IsActive = true,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!Secure")
                };
                
                await coreDb.Users.AddAsync(adminUser);
                await coreDb.SaveChangesAsync();
                logger.LogInformation("Default Administrator account created (admin@tradeoffstack.com / Admin123!Secure).");
            }

            if (!await coreDb.Users.AnyAsync(u => u.Email == "tester@tradeoffstack.com"))
            {
                logger.LogInformation("Creating default Tester account...");
                var testerUser = new TradeOffStackAPI.Models.User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "App",
                    LastName = "Tester",
                    Email = "tester@tradeoffstack.com",
                    Role = TradeOffStackAPI.Models.UserRole.Tester,
                    IsActive = true,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Tester123!Secure")
                };
                
                await coreDb.Users.AddAsync(testerUser);
                await coreDb.SaveChangesAsync();
                logger.LogInformation("Default Tester account created (tester@tradeoffstack.com / Tester123!Secure).");
            }
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Database migration failed. The application cannot start.");
            throw; // Fail-fast: the application must not start with an inconsistent schema.
        }
    }

    /// <summary>
    /// Maps the API Health Checks endpoints.
    /// Essential for monitoring and lifecycle management (Kubernetes/Docker/LoadBalancers).
    /// </summary>
    /// <param name="app">The API's WebApplication instance.</param>
    /// <returns>The updated WebApplication instance for chaining.</returns>
    public static WebApplication MapApiHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks("/health").AllowAnonymous();
        app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false }).AllowAnonymous();
        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("ready")
        }).AllowAnonymous();

        return app;
    }
}
