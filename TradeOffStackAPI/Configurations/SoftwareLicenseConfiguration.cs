using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradeOffStackAPI.Models;

namespace TradeOffStackAPI.Configurations;

public class SoftwareLicenseConfiguration : IEntityTypeConfiguration<SoftwareLicense>
{
    public void Configure(EntityTypeBuilder<SoftwareLicense> builder)
    {
        builder.ToTable("software_licenses");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name).IsRequired().HasMaxLength(255);
        builder.Property(s => s.LicenseKey).HasMaxLength(500);
        builder.Property(s => s.TotalSeats).IsRequired();
        builder.Property(s => s.Price).HasColumnType("decimal(18,2)");
    }
}
