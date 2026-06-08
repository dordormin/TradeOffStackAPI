using TradeOffStackAPI.Dtos;
using TradeOffStackAPI.Models;

namespace TradeOffStackAPI.Services.Interfaces;

/// <summary>
/// Service interface for managing departments.
/// Encapsulates business logic and ensures separation of concerns.
/// </summary>
public interface IDepartmentService
{
    /// <summary>
    /// Retrieves all departments.
    /// </summary>
    /// <returns>A service response containing a collection of departments.</returns>
    Task<ServiceResponse<IEnumerable<Department>>> GetAllAsync();

    /// <summary>
    /// Retrieves a specific department by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the department.</param>
    /// <returns>A service response containing the requested department.</returns>
    Task<ServiceResponse<Department>> GetByIdAsync(Guid id);

    /// <summary>
    /// Adds a newly created department.
    /// </summary>
    /// <param name="department">The department to add.</param>
    /// <returns>A service response containing the added department.</returns>
    Task<ServiceResponse<Department>> AddDepartmentAsync(Department department);

    /// <summary>
    /// Updates an existing department.
    /// </summary>
    /// <param name="id">The unique identifier of the department to update.</param>
    /// <param name="department">The updated department data.</param>
    /// <returns>A service response containing the updated department.</returns>
    Task<ServiceResponse<Department>> UpdateDepartmentAsync(Guid id, Department department);

    /// <summary>
    /// Deletes a specific department.
    /// </summary>
    /// <param name="id">The unique identifier of the department to delete.</param>
    /// <returns>A service response indicating success or failure.</returns>
    Task<ServiceResponse> DeleteDepartmentAsync(Guid id);
}