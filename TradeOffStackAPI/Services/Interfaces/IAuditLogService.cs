using TradeOffStackAPI.Models;

namespace TradeOffStackAPI.Services.Interfaces;

public interface IAuditLogService
{
    Task<IEnumerable<AuditLog>> GetAllAsync();
    Task<IEnumerable<AuditLog>> GetByEntityAsync(string entityType, Guid entityId);
    Task LogAsync(string entityType, Guid entityId, AuditAction action, string? oldValues = null, string? newValues = null, Guid? performedById = null);
}
