namespace TradeOffStackAPI.Data;

using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Services.Interfaces;

public abstract class AuditableDbContext : DbContext
{
    private static readonly HashSet<string> SensitiveProperties = new(StringComparer.Ordinal)
    {
        "PasswordHash"
    };

    protected readonly ICurrentUserService? _currentUser;

    public AuditableDbContext(DbContextOptions options, ICurrentUserService? currentUser = null)
        : base(options)
    {
        _currentUser = currentUser;
    }

    public DbSet<AuditLog> AuditLogs { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var auditEntries = CaptureAuditEntries();

        var result = await base.SaveChangesAsync(cancellationToken);

        if (auditEntries.Count > 0)
        {
            AuditLogs.AddRange(auditEntries.Select(BuildAuditLog));
            await base.SaveChangesAsync(cancellationToken);
        }

        return result;
    }

    private List<AuditEntry> CaptureAuditEntries()
    {
        ChangeTracker.DetectChanges();

        var entries = new List<AuditEntry>();
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is AuditLog)
                continue;
            if (entry.State is not (EntityState.Added or EntityState.Modified or EntityState.Deleted))
                continue;

            entries.Add(new AuditEntry(entry));
        }

        return entries;
    }

    private AuditLog BuildAuditLog(AuditEntry entry) => new()
    {
        Id = Guid.NewGuid(),
        EntityType = entry.EntityType,
        EntityId = entry.ResolveEntityId(),
        Action = entry.Action,
        OldValues = Serialize(entry.OldValues),
        NewValues = Serialize(entry.NewValues),
        PerformedById = _currentUser?.UserId,
        PerformedAt = DateTime.UtcNow
    };

    private static string? Serialize(IReadOnlyDictionary<string, object?> values) =>
        values.Count == 0 ? null : JsonSerializer.Serialize(values);

    private sealed class AuditEntry
    {
        private readonly EntityEntry _entry;

        public string EntityType { get; }
        public AuditAction Action { get; }
        public Dictionary<string, object?> OldValues { get; } = new();
        public Dictionary<string, object?> NewValues { get; } = new();

        public AuditEntry(EntityEntry entry)
        {
            _entry = entry;
            EntityType = entry.Entity.GetType().Name;
            Action = entry.State switch
            {
                EntityState.Added => AuditAction.Created,
                EntityState.Deleted => AuditAction.Deleted,
                _ => AuditAction.Updated
            };

            foreach (var prop in entry.Properties)
            {
                if (SensitiveProperties.Contains(prop.Metadata.Name))
                    continue;

                switch (Action)
                {
                    case AuditAction.Created:
                        NewValues[prop.Metadata.Name] = prop.CurrentValue;
                        break;
                    case AuditAction.Deleted:
                        OldValues[prop.Metadata.Name] = prop.OriginalValue;
                        break;
                    case AuditAction.Updated when prop.IsModified:
                        OldValues[prop.Metadata.Name] = prop.OriginalValue;
                        NewValues[prop.Metadata.Name] = prop.CurrentValue;
                        break;
                }
            }
        }

        public Guid ResolveEntityId()
        {
            var keyProperty = _entry.Metadata.FindPrimaryKey()?.Properties.FirstOrDefault();
            if (keyProperty is null)
                return Guid.Empty;

            return _entry.Property(keyProperty.Name).CurrentValue is Guid id ? id : Guid.Empty;
        }
    }
}
