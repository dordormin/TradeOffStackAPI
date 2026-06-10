using TradeOffStackAPI.Models;

namespace TradeOffStackAPI.Repositories.Interfaces;

/// <summary>
/// Repository interface for managing Reservation entities.
/// </summary>
public interface IReservationRepository : IGenericRepository<Reservation>
{
    /// <summary>
    /// Retrieves all reservations associated with a specific piece of equipment.
    /// </summary>
    /// <param name="equipmentId">The unique identifier of the equipment.</param>
    /// <returns>A collection of reservations for the specified equipment.</returns>
    Task<IEnumerable<Reservation>> GetByEquipmentAsync(Guid equipmentId);

    /// <summary>
    /// Retrieves all reservations made by a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A collection of reservations made by the specified user.</returns>
    Task<IEnumerable<Reservation>> GetByUserAsync(Guid userId);

    /// <summary>
    /// Retrieves all reservations made by a list of users. Useful for cross-service queries.
    /// </summary>
    /// <param name="userIds">A collection of user IDs.</param>
    Task<IEnumerable<Reservation>> GetByUserIdsAsync(IEnumerable<Guid> userIds);

    /// <summary>
    /// Retrieves all currently active reservations.
    /// </summary>
    /// <returns>A collection of active reservations.</returns>
    Task<IEnumerable<Reservation>> GetActiveAsync();

    /// <summary>
    /// Checks if a piece of equipment has any currently active or pending reservations.
    /// </summary>
    /// <param name="equipmentId">The unique identifier of the equipment to check.</param>
    /// <returns>True if an active or pending reservation exists, otherwise false.</returns>
    Task<bool> HasActiveReservationAsync(Guid equipmentId);
}
