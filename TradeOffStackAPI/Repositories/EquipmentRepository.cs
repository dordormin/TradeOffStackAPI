using Microsoft.EntityFrameworkCore;
using TradeOffStackAPI.Data;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Repositories.Interfaces;

namespace TradeOffStackAPI.Repositories;

/// <summary>
/// Repository implementation for Equipment entities.
/// </summary>
public class EquipmentRepository : GenericRepository<Equipment, AssetDbContext>, IEquipmentRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EquipmentRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public EquipmentRepository(AssetDbContext context) : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Equipment>> GetByStatusAsync(AssetStatus status) =>
        await _dbSet.Include(e => e.EquipmentLicenses).ThenInclude(el => el.SoftwareLicense)
                    .Where(e => e.Status == status).ToListAsync();

    /// <inheritdoc />
    public async Task<IEnumerable<Equipment>> GetByCategoryAsync(AssetCategory category) =>
        await _dbSet.Include(e => e.EquipmentLicenses).ThenInclude(el => el.SoftwareLicense)
                    .Where(e => e.Category == category).ToListAsync();

    public override async Task<IEnumerable<Equipment>> GetAllAsync()
    {
        return await _dbSet.Include(e => e.EquipmentLicenses).ThenInclude(el => el.SoftwareLicense).ToListAsync();
    }

    public override async Task<Equipment?> GetByIdAsync(Guid id)
    {
        return await _dbSet.Include(e => e.EquipmentLicenses).ThenInclude(el => el.SoftwareLicense)
                           .FirstOrDefaultAsync(e => e.Id == id);
    }
}
