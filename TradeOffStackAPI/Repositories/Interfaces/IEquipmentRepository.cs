using TradeOffStackAPI.Models;

namespace TradeOffStackAPI.Repositories.Interfaces;

/// <summary>
/// Repository interface for managing Equipment entities.
/// Provides additional specific data access methods for equipment.
/// </summary>
public interface IEquipmentRepository : IGenericRepository<Equipment>
{
    /// <summary>
    /// Retrieves all equipment matching a specific status.
    /// </summary>
    /// <param name="status">The operational status of the equipment.</param>
    /// <returns>A collection of equipment with the specified status.</returns>
    Task<IEnumerable<Equipment>> GetByStatusAsync(AssetStatus status);

    /// <summary>
    /// Retrieves all equipment belonging to a specific category.
    /// </summary>
    /// <param name="category">The category to filter by.</param>
    /// <returns>A collection of equipment in the specified category.</returns>
    Task<IEnumerable<Equipment>> GetByCategoryAsync(AssetCategory category);
}
