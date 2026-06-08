namespace TradeOffStackAPI.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradeOffStackAPI.Models;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("audit_logs");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.EntityType).IsRequired().HasMaxLength(100);

        // Enum natif PostgreSQL : pas de HasColumnType (résolu via MapEnum + HasPostgresEnum).
        builder.Property(a => a.Action)
            .HasColumnName("Action")
            .IsRequired();

        builder.Property(a => a.OldValues).HasColumnType("jsonb");
        builder.Property(a => a.NewValues).HasColumnType("jsonb");
        builder.Property(a => a.PerformedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(a => a.PerformedBy)
            .WithMany()
            .HasForeignKey(a => a.PerformedById)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
