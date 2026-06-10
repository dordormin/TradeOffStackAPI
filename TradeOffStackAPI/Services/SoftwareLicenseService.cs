using TradeOffStackAPI.Dtos;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Repositories.Interfaces;
using TradeOffStackAPI.Services.Interfaces;

namespace TradeOffStackAPI.Services;

public class SoftwareLicenseService : ISoftwareLicenseService
{
    private readonly ISoftwareLicenseRepository _repo;

    public SoftwareLicenseService(ISoftwareLicenseRepository repo)
    {
        _repo = repo;
    }

    public async Task<ServiceResponse<IEnumerable<SoftwareLicense>>> GetAllAsync()
    {
        var licenses = await _repo.GetAllAsync();
        return ServiceResponse<IEnumerable<SoftwareLicense>>.Ok(licenses);
    }

    public async Task<ServiceResponse<SoftwareLicense>> GetByIdAsync(Guid id)
    {
        var license = await _repo.GetByIdAsync(id);
        return license != null ? ServiceResponse<SoftwareLicense>.Ok(license) : ServiceResponse<SoftwareLicense>.Fail("License not found.");
    }

    public async Task<ServiceResponse<IEnumerable<SoftwareLicense>>> GetActiveLicensesAsync()
    {
        var licenses = await _repo.GetActiveLicensesAsync();
        return ServiceResponse<IEnumerable<SoftwareLicense>>.Ok(licenses);
    }

    public async Task<ServiceResponse<SoftwareLicense>> AddAsync(SoftwareLicense license)
    {
        if (string.IsNullOrWhiteSpace(license.Name))
            return ServiceResponse<SoftwareLicense>.Fail("License name is required.");
            
        var success = await _repo.AddAsync(license);
        return success ? ServiceResponse<SoftwareLicense>.Ok(license) : ServiceResponse<SoftwareLicense>.Fail("Failed to add license.");
    }

    public async Task<ServiceResponse<SoftwareLicense>> UpdateAsync(Guid id, SoftwareLicense license)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null)
            return ServiceResponse<SoftwareLicense>.Fail("License not found.");

        existing.Name = license.Name;
        existing.LicenseKey = license.LicenseKey;
        existing.TotalSeats = license.TotalSeats;
        existing.ExpirationDate = license.ExpirationDate;
        existing.Price = license.Price;

        var success = await _repo.UpdateAsync(existing);
        return success ? ServiceResponse<SoftwareLicense>.Ok(existing) : ServiceResponse<SoftwareLicense>.Fail("Failed to update license.");
    }

    public async Task<ServiceResponse> DeleteAsync(Guid id)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) return ServiceResponse.Fail("License not found.");

        var success = await _repo.DeleteAsync(id);
        return success ? ServiceResponse.Ok("License deleted.") : ServiceResponse.Fail("Failed to delete license.");
    }
}
