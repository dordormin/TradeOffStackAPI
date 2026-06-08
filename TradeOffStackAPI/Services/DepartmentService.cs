using TradeOffStackAPI.Dtos;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Repositories.Interfaces;
using TradeOffStackAPI.Services.Interfaces;

namespace TradeOffStackAPI.Services;

/// <summary>
/// Service implementation for managing departments.
/// </summary>
public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _repo;

    /// <summary>
    /// Initializes a new instance of the <see cref="DepartmentService"/> class.
    /// </summary>
    /// <param name="repo">The department repository.</param>
    public DepartmentService(IDepartmentRepository repo)
    {
        _repo = repo;
    }

    /// <inheritdoc />
    public async Task<ServiceResponse<IEnumerable<Department>>> GetAllAsync()
    {
        var departments = await _repo.GetAllAsync();
        return ServiceResponse<IEnumerable<Department>>.Ok(departments);
    }

    /// <inheritdoc />
    public async Task<ServiceResponse<Department>> GetByIdAsync(Guid id)
    {
        var department = await _repo.GetByIdAsync(id);
        return department != null
            ? ServiceResponse<Department>.Ok(department)
            : ServiceResponse<Department>.Fail("Department not found.");
    }

    /// <inheritdoc />
    public async Task<ServiceResponse<Department>> AddDepartmentAsync(Department department)
    {
        if (string.IsNullOrEmpty(department.Name))
            return ServiceResponse<Department>.Fail("Department name is required.");

        var success = await _repo.AddAsync(department);
        return success
            ? ServiceResponse<Department>.Ok(department, "Department successfully created.")
            : ServiceResponse<Department>.Fail("Failed to save the department.");
    }

    /// <inheritdoc />
    public async Task<ServiceResponse<Department>> UpdateDepartmentAsync(Guid id, Department department)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null)
            return ServiceResponse<Department>.Fail("Department not found.");

        existing.Name = department.Name;
        existing.Description = department.Description;

        var success = await _repo.UpdateAsync(existing);
        return success
            ? ServiceResponse<Department>.Ok(existing, "Department successfully updated.")
            : ServiceResponse<Department>.Fail("Failed to update the department.");
    }

    /// <inheritdoc />
    public async Task<ServiceResponse> DeleteDepartmentAsync(Guid id)
    {
        var success = await _repo.DeleteAsync(id);
        return success
            ? ServiceResponse.Ok("Department successfully deleted.")
            : ServiceResponse.Fail("Department not found or failed to delete.");
    }
}