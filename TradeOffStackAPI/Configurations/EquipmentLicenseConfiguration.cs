using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradeOffStackAPI.Models;

namespace TradeOffStackAPI.Configurations;

public class EquipmentLicenseConfiguration : IEntityTypeConfiguration<EquipmentLicense>
{
    public void Configure(EntityTypeBuilder<EquipmentLicense> builder)
    {
        builder.ToTable("equipment_licenses");
        
        // Composite Primary Key
        builder.HasKey(el => new { el.EquipmentId, el.SoftwareLicenseId });

        builder.HasOne(el => el.Equipment)
            .WithMany(e => e.EquipmentLicenses)
            .HasForeignKey(el => el.EquipmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(el => el.SoftwareLicense)
            .WithMany(s => s.EquipmentLicenses)
            .HasForeignKey(el => el.SoftwareLicenseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
