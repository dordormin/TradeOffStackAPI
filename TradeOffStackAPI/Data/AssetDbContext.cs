namespace TradeOffStackAPI.Data;

using Microsoft.EntityFrameworkCore;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Services.Interfaces;

public class AssetDbContext : AuditableDbContext
{
    public AssetDbContext(DbContextOptions<AssetDbContext> options, ICurrentUserService? currentUser = null)
        : base(options, currentUser)
    {
    }

    public DbSet<Equipment> Equipments { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }
    public DbSet<SoftwareLicense> SoftwareLicenses { get; set; }
    public DbSet<EquipmentLicense> EquipmentLicenses { get; set; }
    
    // SaaS Management module
    public DbSet<SaaSProvider> SaaSProviders { get; set; }
    public DbSet<SaaSSubscription> SaaSSubscriptions { get; set; }
    public DbSet<SaaSAssignment> SaaSAssignments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Register PostgreSQL Enums with PascalCase translator to match runtime config
        var translator = new Npgsql.NameTranslation.NpgsqlNullNameTranslator();
        modelBuilder.HasPostgresEnum<AssetStatus>(name: "asset_status", nameTranslator: translator);
        modelBuilder.HasPostgresEnum<AssetCategory>(name: "asset_category", nameTranslator: translator);
        modelBuilder.HasPostgresEnum<ReservationStatus>(name: "reservation_status", nameTranslator: translator);
        modelBuilder.HasPostgresEnum<MaintenanceStatus>(name: "maintenance_status", nameTranslator: translator);
        modelBuilder.HasPostgresEnum<MaintenancePriority>(name: "maintenance_priority", nameTranslator: translator);
        modelBuilder.HasPostgresEnum<AuditAction>(name: "audit_action", nameTranslator: translator);
        modelBuilder.HasPostgresEnum<DepreciationMethod>(name: "depreciation_method", nameTranslator: translator);

        // Register specific configurations to prevent configuration leakage between contexts
        modelBuilder.ApplyConfiguration(new TradeOffStackAPI.Data.Configurations.EquipmentConfiguration());
        modelBuilder.ApplyConfiguration(new TradeOffStackAPI.Data.Configurations.ReservationConfiguration());
        modelBuilder.ApplyConfiguration(new TradeOffStackAPI.Data.Configurations.MaintenanceRequestConfiguration());
        modelBuilder.ApplyConfiguration(new TradeOffStackAPI.Configurations.SoftwareLicenseConfiguration());
        modelBuilder.ApplyConfiguration(new TradeOffStackAPI.Configurations.EquipmentLicenseConfiguration());
        modelBuilder.ApplyConfiguration(new TradeOffStackAPI.Data.Configurations.AuditLogConfiguration());
    }
}
