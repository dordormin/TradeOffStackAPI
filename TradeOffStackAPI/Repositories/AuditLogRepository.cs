using Microsoft.EntityFrameworkCore;
using TradeOffStackAPI.Data;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Repositories.Interfaces;

namespace TradeOffStackAPI.Repositories;

/// <summary>
/// Repository implementation for AuditLog entities.
/// Overrides generic methods to sort logs chronologically.
/// </summary>
public class AuditLogRepository : GenericRepository<AuditLog>, IAuditLogRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuditLogRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public AuditLogRepository(AppDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Retrieves all audit logs, eagerly loading the user who performed the action,
    /// and sorts them by date descending (newest first).
    /// </summary>
    /// <returns>A collection of audit logs ordered chronologically descending.</returns>
    public override async Task<IEnumerable<AuditLog>> GetAllAsync() =>
        await _dbSet
            .Include(a => a.PerformedBy)
            .OrderByDescending(a => a.PerformedAt)
            .ToListAsync();

    /// <inheritdoc />
    public async Task<IEnumerable<AuditLog>> GetByEntityAsync(string entityType, Guid entityId) =>
        await _dbSet
            .Include(a => a.PerformedBy)
            .Where(a => a.EntityType == entityType && a.EntityId == entityId)
            .OrderByDescending(a => a.PerformedAt)
            .ToListAsync();
}
