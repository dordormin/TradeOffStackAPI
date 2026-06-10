using Microsoft.EntityFrameworkCore;
using TradeOffStackAPI.Data;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Repositories.Interfaces;

namespace TradeOffStackAPI.Repositories;

/// <summary>
/// Repository implementation for Reservation entities.
/// Overrides generic methods to include relational data (Equipment, User).
/// </summary>
public class ReservationRepository : GenericRepository<Reservation, AssetDbContext>, IReservationRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ReservationRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ReservationRepository(AssetDbContext context) : base(context)
    {
    }

    /// <inheritdoc />
    public override async Task<IEnumerable<Reservation>> GetAllAsync() =>
        await _dbSet
            .Include(r => r.Equipment)
            .ToListAsync();

    /// <inheritdoc />
    public override async Task<Reservation?> GetByIdAsync(Guid id) =>
        await _dbSet
            .Include(r => r.Equipment)
            .FirstOrDefaultAsync(r => r.Id == id);

    /// <inheritdoc />
    public async Task<IEnumerable<Reservation>> GetByEquipmentAsync(Guid equipmentId) =>
        await _dbSet
            .Where(r => r.EquipmentId == equipmentId)
            .ToListAsync();

    /// <inheritdoc />
    public async Task<IEnumerable<Reservation>> GetByUserAsync(Guid userId) =>
        await _dbSet
            .Include(r => r.Equipment)
            .Where(r => r.UserId == userId)
            .ToListAsync();

    /// <inheritdoc />
    // Data protection: reservations where the user belongs to the given department.
    public async Task<IEnumerable<Reservation>> GetByUserIdsAsync(IEnumerable<Guid> userIds) =>
        await _dbSet
            .Include(r => r.Equipment)
            .Where(r => userIds.Contains(r.UserId))
            .ToListAsync();

    /// <inheritdoc />
    public async Task<IEnumerable<Reservation>> GetActiveAsync() =>
        await _dbSet
            .Include(r => r.Equipment)
            .Where(r => r.Status == ReservationStatus.Active)
            .ToListAsync();

    /// <inheritdoc />
    public async Task<bool> HasActiveReservationAsync(Guid equipmentId) =>
        await _dbSet.AnyAsync(r =>
            r.EquipmentId == equipmentId &&
            (r.Status == ReservationStatus.Pending || r.Status == ReservationStatus.Active));
}
