using TradeOffStackAPI.Models;

namespace TradeOffStackAPI.Repositories.Interfaces;

/// <summary>
/// Repository interface for managing MaintenanceRequest entities.
/// </summary>
public interface IMaintenanceRequestRepository : IGenericRepository<MaintenanceRequest>
{
    /// <summary>
    /// Retrieves all maintenance requests associated with a specific piece of equipment.
    /// </summary>
    /// <param name="equipmentId">The unique identifier of the equipment.</param>
    /// <returns>A collection of maintenance requests for the specified equipment.</returns>
    Task<IEnumerable<MaintenanceRequest>> GetByEquipmentAsync(Guid equipmentId);

    /// <summary>
    /// Retrieves all maintenance requests matching a specific status.
    /// </summary>
    /// <param name="status">The current status of the maintenance request.</param>
    /// <returns>A collection of maintenance requests with the specified status.</returns>
    Task<IEnumerable<MaintenanceRequest>> GetByStatusAsync(MaintenanceStatus status);

    /// <summary>
    /// Checks if a piece of equipment has any currently open (pending or in-progress) maintenance requests.
    /// </summary>
    /// <param name="equipmentId">The unique identifier of the equipment to check.</param>
    /// <returns>True if an open maintenance request exists, otherwise false.</returns>
    Task<bool> HasOpenRequestAsync(Guid equipmentId);
}
