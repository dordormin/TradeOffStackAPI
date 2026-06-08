using Microsoft.EntityFrameworkCore;
using TradeOffStackAPI.Data;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Repositories.Interfaces;

namespace TradeOffStackAPI.Repositories;

/// <summary>
/// Repository implementation for MaintenanceRequest entities.
/// Overrides generic methods to include relational data (Equipment, RequestedBy).
/// </summary>
public class MaintenanceRequestRepository : GenericRepository<MaintenanceRequest>, IMaintenanceRequestRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MaintenanceRequestRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public MaintenanceRequestRepository(AppDbContext context) : base(context)
    {
    }

    /// <inheritdoc />
    public override async Task<IEnumerable<MaintenanceRequest>> GetAllAsync() =>
        await _dbSet
            .Include(m => m.Equipment)
            .Include(m => m.RequestedBy)
            .ToListAsync();

    /// <inheritdoc />
    public override async Task<MaintenanceRequest?> GetByIdAsync(Guid id) =>
        await _dbSet
            .Include(m => m.Equipment)
            .Include(m => m.RequestedBy)
            .FirstOrDefaultAsync(m => m.Id == id);

    /// <inheritdoc />
    public async Task<IEnumerable<MaintenanceRequest>> GetByEquipmentAsync(Guid equipmentId) =>
        await _dbSet
            .Include(m => m.RequestedBy)
            .Where(m => m.EquipmentId == equipmentId)
            .ToListAsync();

    /// <inheritdoc />
    public async Task<IEnumerable<MaintenanceRequest>> GetByStatusAsync(MaintenanceStatus status) =>
        await _dbSet
            .Include(m => m.Equipment)
            .Include(m => m.RequestedBy)
            .Where(m => m.Status == status)
            .ToListAsync();

    /// <inheritdoc />
    public async Task<bool> HasOpenRequestAsync(Guid equipmentId) =>
        await _dbSet.AnyAsync(m =>
            m.EquipmentId == equipmentId &&
            (m.Status == MaintenanceStatus.Pending || m.Status == MaintenanceStatus.InProgress));
}
