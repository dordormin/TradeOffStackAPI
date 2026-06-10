using TradeOffStackAPI.Models;

namespace TradeOffStackAPI.Repositories.Interfaces;

public interface ISoftwareLicenseRepository : IGenericRepository<SoftwareLicense>
{
    Task<IEnumerable<SoftwareLicense>> GetActiveLicensesAsync();
}
