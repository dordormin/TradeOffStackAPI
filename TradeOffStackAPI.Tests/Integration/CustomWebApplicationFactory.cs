using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TradeOffStackAPI.Data;
using Microsoft.Data.Sqlite;
using System.Data.Common;
using TradeOffStackAPI.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TradeOffStackAPI.Services.Interfaces;
using System.Linq;
using System;

namespace TradeOffStackAPI.Tests.Integration;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly DbConnection _coreConnection;
    private readonly DbConnection _assetConnection;

    public CustomWebApplicationFactory()
    {
        _coreConnection = new SqliteConnection("DataSource=:memory:");
        _coreConnection.Open();
        _assetConnection = new SqliteConnection("DataSource=:memory:");
        _assetConnection.Open();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureAppConfiguration((context, config) => 
        {
            config.AddInMemoryCollection(new Dictionary<string, string?> 
            {
                ["ConnectionStrings:CoreConnection"] = "Host=localhost;Database=dummy",
                ["ConnectionStrings:AssetConnection"] = "Host=localhost;Database=dummy"
            });
        });

        builder.ConfigureServices(services =>
        {
            var descriptorsToRemove = services
                .Where(d =>
                    d.ServiceType == typeof(DbContextOptions<CoreDbContext>) ||
                    d.ServiceType == typeof(CoreDbContext) ||
                    d.ServiceType == typeof(DbContextOptions<AssetDbContext>) ||
                    d.ServiceType == typeof(AssetDbContext) ||
                    d.ServiceType.FullName?.Contains("Npgsql") == true)
                .ToList();

            foreach (var descriptor in descriptorsToRemove)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<CoreDbContext, TestCoreDbContext>(options =>
            {
                options.UseSqlite(_coreConnection);
            });

            services.AddScoped<DbContextOptions<CoreDbContext>>(sp =>
                new DbContextOptionsBuilder<CoreDbContext>()
                    .UseSqlite(_coreConnection)
                    .Options);

            services.AddDbContext<AssetDbContext, TestAssetDbContext>(options =>
            {
                options.UseSqlite(_assetConnection);
            });

            services.AddScoped<DbContextOptions<AssetDbContext>>(sp =>
                new DbContextOptionsBuilder<AssetDbContext>()
                    .UseSqlite(_assetConnection)
                    .Options);

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            
            var coreDb = scope.ServiceProvider.GetRequiredService<CoreDbContext>();
            coreDb.Database.EnsureCreated();
            
            var assetDb = scope.ServiceProvider.GetRequiredService<AssetDbContext>();
            assetDb.Database.EnsureCreated();
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _coreConnection.Close();
        _coreConnection.Dispose();
        _assetConnection.Close();
        _assetConnection.Dispose();
    }
}

public class TestCoreDbContext : CoreDbContext
{
    public TestCoreDbContext(DbContextOptions<CoreDbContext> options, ICurrentUserService? currentUser = null)
        : base(options, currentUser)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        ApplySqliteFixes(modelBuilder);
    }

    private void ApplySqliteFixes(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType.IsEnum)
                {
                    var converterType = typeof(EnumToStringConverter<>).MakeGenericType(property.ClrType);
                    var converter = (ValueConverter)Activator.CreateInstance(converterType)!;
                    property.SetValueConverter(converter);
                }
            }
        }
    }
}

public class TestAssetDbContext : AssetDbContext
{
    public TestAssetDbContext(DbContextOptions<AssetDbContext> options, ICurrentUserService? currentUser = null)
        : base(options, currentUser)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        ApplySqliteFixes(modelBuilder);
    }

    private void ApplySqliteFixes(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType.IsEnum)
                {
                    var converterType = typeof(EnumToStringConverter<>).MakeGenericType(property.ClrType);
                    var converter = (ValueConverter)Activator.CreateInstance(converterType)!;
                    property.SetValueConverter(converter);
                }
            }
        }
    }
}
