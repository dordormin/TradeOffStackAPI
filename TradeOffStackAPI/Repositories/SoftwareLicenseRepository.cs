using Microsoft.EntityFrameworkCore;
using TradeOffStackAPI.Data;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Repositories.Interfaces;

namespace TradeOffStackAPI.Repositories;

public class SoftwareLicenseRepository : GenericRepository<SoftwareLicense, AssetDbContext>, ISoftwareLicenseRepository
{
    public SoftwareLicenseRepository(AssetDbContext context) : base(context) { }

    public async Task<IEnumerable<SoftwareLicense>> GetActiveLicensesAsync()
    {
        return await _dbSet
            .Where(s => s.ExpirationDate == null || s.ExpirationDate > DateTime.UtcNow)
            .ToListAsync();
    }
}
