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
        // Only apply configurations from this assembly that relate to AssetPortal
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssetDbContext).Assembly);
    }
}
