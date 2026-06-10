using TradeOffStackAPI.Dtos;
using TradeOffStackAPI.Models;

namespace TradeOffStackAPI.Services.Interfaces;

public interface ISoftwareLicenseService
{
    Task<ServiceResponse<IEnumerable<SoftwareLicense>>> GetAllAsync();
    Task<ServiceResponse<SoftwareLicense>> GetByIdAsync(Guid id);
    Task<ServiceResponse<IEnumerable<SoftwareLicense>>> GetActiveLicensesAsync();
    Task<ServiceResponse<SoftwareLicense>> AddAsync(SoftwareLicense license);
    Task<ServiceResponse<SoftwareLicense>> UpdateAsync(Guid id, SoftwareLicense license);
    Task<ServiceResponse> DeleteAsync(Guid id);
}
