using Microsoft.EntityFrameworkCore;
using TradeOffStackAPI.Data;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Repositories.Interfaces;

namespace TradeOffStackAPI.Repositories;

/// <summary>
/// Repository implementation for Equipment entities.
/// </summary>
public class EquipmentRepository : GenericRepository<Equipment>, IEquipmentRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EquipmentRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public EquipmentRepository(AppDbContext context) : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Equipment>> GetByStatusAsync(AssetStatus status) =>
        await _dbSet.Where(e => e.Status == status).ToListAsync();

    /// <inheritdoc />
    public async Task<IEnumerable<Equipment>> GetByCategoryAsync(AssetCategory category) =>
        await _dbSet.Where(e => e.Category == category).ToListAsync();
}
