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
        // Only apply configurations from this assembly that relate to Core
        // For simplicity, we just apply all configurations since EF ignores irrelevant ones usually,
        // but it's safer to only apply specific ones if we had them separated.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoreDbContext).Assembly);
    }
}