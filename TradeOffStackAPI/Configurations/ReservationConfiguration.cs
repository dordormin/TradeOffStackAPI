namespace TradeOffStackAPI.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradeOffStackAPI.Models;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("reservations");
        builder.HasKey(r => r.Id);

        // Enum natif PostgreSQL : pas de HasColumnType (résolu via MapEnum + HasPostgresEnum).
        builder.Property(r => r.Status)
            .HasColumnName("Status")
            .IsRequired();

        builder.Property(r => r.StartDate).IsRequired();
        builder.Property(r => r.Notes).HasColumnType("text");
        builder.Property(r => r.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(r => r.Equipment)
            .WithMany(e => e.Reservations)
            .HasForeignKey(r => r.EquipmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
