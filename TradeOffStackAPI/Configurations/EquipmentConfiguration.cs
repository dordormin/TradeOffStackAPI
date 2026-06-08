namespace TradeOffStackAPI.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradeOffStackAPI.Models;

public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
{
    public void Configure(EntityTypeBuilder<Equipment> builder)
    {
        builder.ToTable("equipments");
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Name).IsRequired().HasMaxLength(255);
        builder.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        // numeric(18,2) : stockage monétaire exact.
        builder.Property(e => e.Price).HasColumnType("numeric(18,2)");

        // ENUMs PostgreSQL :
        // On NE met PAS HasColumnType ici. Le type natif (asset_status / asset_category)
        // est résolu automatiquement par la combinaison MapEnum<T> (NpgsqlDataSourceBuilder,
        // dans Program.cs) + HasPostgresEnum<T> (AppDbContext). Forcer HasColumnType
        // réintroduit un mapping "text" et provoque l'erreur PostgreSQL 42804
        // (« column is of type asset_status but expression is of type text »).
        //
        // Les colonnes ont été générées par EF en PascalCase quoté ("Status"/"Category"),
        // d'où le HasColumnName explicite qui doit conserver cette casse exacte.
        builder.Property(e => e.Status)
            .HasColumnName("Status")
            .IsRequired();

        builder.Property(e => e.Category)
            .HasColumnName("Category")
            .IsRequired();
    }
}