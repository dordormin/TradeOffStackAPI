using TradeOffStackAPI.Models;

namespace TradeOffStackAPI.Repositories.Interfaces;

/// <summary>
/// Repository interface for managing User entities.
/// </summary>
public interface IUserRepository : IGenericRepository<User>
{
    /// <summary>
    /// Retrieves a user by their exact email address.
    /// </summary>
    /// <param name="email">The email address to search for.</param>
    /// <returns>The found user, or null if no user matches.</returns>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>
    /// Retrieves all users belonging to a specific department.
    /// </summary>
    /// <param name="departmentId">The unique identifier of the department.</param>
    /// <returns>A collection of users in the specified department.</returns>
    Task<IEnumerable<User>> GetByDepartmentAsync(Guid departmentId);
}
