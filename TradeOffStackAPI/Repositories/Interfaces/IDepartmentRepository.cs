using TradeOffStackAPI.Models;

namespace TradeOffStackAPI.Repositories.Interfaces;

/// <summary>
/// Repository interface for managing Department entities.
/// Inherits from the generic repository to provide standard CRUD operations.
/// </summary>
public interface IDepartmentRepository : IGenericRepository<Department>
{
}
