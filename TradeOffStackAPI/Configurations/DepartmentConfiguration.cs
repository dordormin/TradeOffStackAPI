namespace TradeOffStackAPI.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradeOffStackAPI.Models;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("departments");
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name).IsRequired().HasMaxLength(255);
        builder.Property(d => d.Description).HasColumnType("text");
        builder.Property(d => d.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
