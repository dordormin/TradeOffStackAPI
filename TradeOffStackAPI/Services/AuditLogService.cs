using TradeOffStackAPI.Models;
using TradeOffStackAPI.Repositories.Interfaces;
using TradeOffStackAPI.Services.Interfaces;

namespace TradeOffStackAPI.Services;

public class AuditLogService : IAuditLogService
{
    private readonly IAuditLogRepository _repo;

    public AuditLogService(IAuditLogRepository repo)
    {
        _repo = repo;
    }

    /// <inheritdoc />

    public async Task<IEnumerable<AuditLog>> GetAllAsync() => await _repo.GetAllAsync();

    /// <inheritdoc />

    public async Task<IEnumerable<AuditLog>> GetByEntityAsync(string entityType, Guid entityId) =>
        await _repo.GetByEntityAsync(entityType, entityId);

    /// <inheritdoc />

    public async Task LogAsync(string entityType, Guid entityId, AuditAction action,
        string? oldValues = null, string? newValues = null, Guid? performedById = null)
    {
        var log = new AuditLog
        {
            Id = Guid.NewGuid(),
            EntityType = entityType,
            EntityId = entityId,
            Action = action,
            OldValues = oldValues,
            NewValues = newValues,
            PerformedById = performedById,
            PerformedAt = DateTime.UtcNow
        };

        await _repo.AddAsync(log);
    }
}
