using TradeOffStackAPI.Models;

namespace TradeOffStackAPI.Repositories.Interfaces;

/// <summary>
/// Repository interface for managing AuditLog entities.
/// </summary>
public interface IAuditLogRepository : IGenericRepository<AuditLog>
{
    /// <summary>
    /// Retrieves all audit logs associated with a specific entity (e.g., a specific Equipment or User).
    /// </summary>
    /// <param name="entityType">The string representation of the entity type (e.g., "Equipment").</param>
    /// <param name="entityId">The unique identifier of the entity.</param>
    /// <returns>A collection of audit logs ordered chronologically descending.</returns>
    Task<IEnumerable<AuditLog>> GetByEntityAsync(string entityType, Guid entityId);
}
