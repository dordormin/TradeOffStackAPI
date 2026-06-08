namespace TradeOffStackAPI.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradeOffStackAPI.Models;

public class MaintenanceRequestConfiguration : IEntityTypeConfiguration<MaintenanceRequest>
{
    public void Configure(EntityTypeBuilder<MaintenanceRequest> builder)
    {
        builder.ToTable("maintenance_requests");
        builder.HasKey(m => m.Id);

        // Enums natifs PostgreSQL : pas de HasColumnType (résolu via MapEnum + HasPostgresEnum).
        builder.Property(m => m.Status)
            .HasColumnName("Status")
            .IsRequired();

        builder.Property(m => m.Priority)
            .HasColumnName("Priority")
            .IsRequired();

        builder.Property(m => m.Description).IsRequired().HasColumnType("text");
        builder.Property(m => m.TechnicianNotes).HasColumnType("text");
        builder.Property(m => m.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(m => m.Equipment)
            .WithMany(e => e.MaintenanceRequests)
            .HasForeignKey(m => m.EquipmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.RequestedBy)
            .WithMany(u => u.MaintenanceRequests)
            .HasForeignKey(m => m.RequestedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
