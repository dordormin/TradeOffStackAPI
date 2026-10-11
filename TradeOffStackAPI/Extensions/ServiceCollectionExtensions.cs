using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Npgsql.NameTranslation;
using StackExchange.Redis;
using TradeOffStackAPI.Auth;
using TradeOffStackAPI.Data;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Repositories;
using TradeOffStackAPI.Repositories.Interfaces;
using TradeOffStackAPI.Services;
using TradeOffStackAPI.Services.Interfaces;

namespace TradeOffStackAPI.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var coreConnectionString = configuration.GetConnectionString("CoreConnection");
        var assetConnectionString = configuration.GetConnectionString("AssetConnection");

        var noTranslation = new NpgsqlNullNameTranslator();
        
        // Core Data Source
        if (string.IsNullOrEmpty(coreConnectionString)) return services; var coreDataSourceBuilder = new NpgsqlDataSourceBuilder(coreConnectionString);
        coreDataSourceBuilder.MapEnum<UserRole>("user_role", noTranslation);
        coreDataSourceBuilder.MapEnum<AuditAction>("audit_action", noTranslation);
        var coreDataSource = coreDataSourceBuilder.Build();

        // Asset Portal Data Source
        var assetDataSourceBuilder = new NpgsqlDataSourceBuilder(assetConnectionString);
        assetDataSourceBuilder.MapEnum<AssetStatus>("asset_status", noTranslation);
        assetDataSourceBuilder.MapEnum<AssetCategory>("asset_category", noTranslation);
        assetDataSourceBuilder.MapEnum<ReservationStatus>("reservation_status", noTranslation);
        assetDataSourceBuilder.MapEnum<MaintenanceStatus>("maintenance_status", noTranslation);
        assetDataSourceBuilder.MapEnum<MaintenancePriority>("maintenance_priority", noTranslation);
        assetDataSourceBuilder.MapEnum<AuditAction>("audit_action", noTranslation);
        assetDataSourceBuilder.MapEnum<DepreciationMethod>("depreciation_method", noTranslation);
        var assetDataSource = assetDataSourceBuilder.Build();

        services.AddDbContext<CoreDbContext>(options =>
            options.UseNpgsql(coreDataSource, o =>
            {
                o.EnableRetryOnFailure(
                    maxRetryCount: 3, 
                    maxRetryDelay: TimeSpan.FromSeconds(5), 
                    errorCodesToAdd: null);
            }));

        services.AddDbContext<AssetDbContext>(options =>
            options.UseNpgsql(assetDataSource, o =>
            {
                o.EnableRetryOnFailure(
                    maxRetryCount: 3, 
                    maxRetryDelay: TimeSpan.FromSeconds(5), 
                    errorCodesToAdd: null);
            }));

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CloudflareR2Settings>(configuration.GetSection(CloudflareR2Settings.SectionName));
        services.AddScoped<IR2UploadService, R2UploadService>();

        services.AddScoped<IEquipmentRepository, EquipmentRepository>();
        services.AddScoped<IEquipmentService, EquipmentService>();
        services.AddScoped<IDepreciationService, DepreciationService>();

        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IDepartmentService, DepartmentService>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IReservationService, ReservationService>();

        services.AddScoped<IMaintenanceRequestRepository, MaintenanceRequestRepository>();
        services.AddScoped<IMaintenanceRequestService, MaintenanceRequestService>();

        services.AddScoped<ISoftwareLicenseRepository, SoftwareLicenseRepository>();
        services.AddScoped<ISoftwareLicenseService, SoftwareLicenseService>();

        services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        services.AddScoped<IAuditLogService, AuditLogService>();

        services.AddScoped<ISaaSService, SaaSService>();

        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        var jwt = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
                  ?? throw new InvalidOperationException("Section de configuration 'Jwt' manquante.");

        // SECURITY: never start with a missing, placeholder or weak signing key.
        // Provide it via the 'Jwt:Key' setting (env var 'Jwt__Key'), min 32 chars.
        if (string.IsNullOrWhiteSpace(jwt.Key)
            || jwt.Key.Length < 32
            || jwt.Key == "YOUR_SUPER_SECRET_JWT_KEY_MINIMUM_32_CHARACTERS_LONG!")
            throw new InvalidOperationException(
                "JWT signing key is missing, too short or still the placeholder. " +
                "Set a strong key via the 'Jwt:Key' configuration value (environment variable 'Jwt__Key', minimum 32 characters).");

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddSingleton<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
                    ClockSkew = TimeSpan.Zero
                };

                // F-05: read the JWT from the HttpOnly 'tos_auth' cookie when no
                // Authorization header is present (browser cookie flow).
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (string.IsNullOrEmpty(context.Token))
                        {
                            var cookieToken = context.Request.Cookies["tos_auth"];
                            if (!string.IsNullOrEmpty(cookieToken))
                                context.Token = cookieToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        return services;
    }

    public static IServiceCollection AddHealthChecksConfiguration(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddDbContextCheck<CoreDbContext>("core_database", tags: new[] { "ready" })
            .AddDbContextCheck<AssetDbContext>("asset_database", tags: new[] { "ready" });

        return services;
    }

    public static IServiceCollection AddScalabilityServices(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnection = configuration.GetConnectionString("Redis");

        if (!string.IsNullOrWhiteSpace(redisConnection))
        {
            var multiplexer = ConnectionMultiplexer.Connect(redisConnection);
            services.AddSingleton<IConnectionMultiplexer>(multiplexer);

            services.AddStackExchangeRedisCache(options => options.Configuration = redisConnection);

            services.AddDataProtection()
                .SetApplicationName("TradeOffStackAPI")
                .PersistKeysToStackExchangeRedis(multiplexer, "DataProtection-Keys");
        }
        else
        {
            services.AddDistributedMemoryCache();
        }

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            options.KnownIPNetworks.Clear();
            options.KnownProxies.Clear();
        });

        return services;
    }
}