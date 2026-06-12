namespace TradeOffStackAPI.Data;

using Microsoft.EntityFrameworkCore;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Services.Interfaces;

public class CoreDbContext : AuditableDbContext
{
    public CoreDbContext(DbContextOptions<CoreDbContext> options, ICurrentUserService? currentUser = null)
        : base(options, currentUser)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Department> Departments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Register PostgreSQL Enums with PascalCase translator to match runtime config
        var translator = new Npgsql.NameTranslation.NpgsqlNullNameTranslator();
        modelBuilder.HasPostgresEnum<UserRole>(name: "user_role", nameTranslator: translator);
        modelBuilder.HasPostgresEnum<AuditAction>(name: "audit_action", nameTranslator: translator);

        // Ignore asset-related navigation properties on User for CoreDbContext
        modelBuilder.Entity<User>().Ignore(u => u.Reservations);
        modelBuilder.Entity<User>().Ignore(u => u.MaintenanceRequests);

        // Register specific configurations to prevent configuration leakage between contexts
        modelBuilder.ApplyConfiguration(new Configurations.UserConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.DepartmentConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.AuditLogConfiguration());
    }
}