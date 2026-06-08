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

namespace TradeOffStackAPI.Tests.Integration;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly DbConnection _connection;

    public CustomWebApplicationFactory()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // Retirer TOUS les descripteurs liés à PostgreSQL / Npgsql / DbContext
            // afin de les remplacer par SQLite pour les tests.
            var descriptorsToRemove = services
                .Where(d =>
                    d.ServiceType == typeof(DbContextOptions<AppDbContext>) ||
                    d.ServiceType == typeof(AppDbContext) ||
                    d.ServiceType.FullName?.Contains("Npgsql") == true ||
                    d.ServiceType.FullName?.Contains("NpgsqlDataSource") == true ||
                    d.ImplementationType?.FullName?.Contains("Npgsql") == true)
                .ToList();

            foreach (var descriptor in descriptorsToRemove)
            {
                services.Remove(descriptor);
            }

            // On ajoute notre DbContext de test qui sait comment gérer les enums avec SQLite
            services.AddDbContext<AppDbContext, TestAppDbContext>(options =>
            {
                options.UseSqlite(_connection);
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _connection.Close();
        _connection.Dispose();
    }
}

// On doit surcharger le DbContext pour les tests afin de lui dire comment gérer les Enums avec SQLite
public class TestAppDbContext : AppDbContext
{
    public TestAppDbContext(DbContextOptions<TestAppDbContext> options, ICurrentUserService? currentUser = null)
        : base(options, currentUser)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Conversion de tous les enums en string pour SQLite
        if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
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
}