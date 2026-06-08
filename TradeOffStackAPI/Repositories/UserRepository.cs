using Microsoft.EntityFrameworkCore;
using TradeOffStackAPI.Data;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Repositories.Interfaces;

namespace TradeOffStackAPI.Repositories;

/// <summary>
/// Repository implementation for User entities.
/// Overrides generic methods to include relational data (e.g., Department).
/// </summary>
public class UserRepository : GenericRepository<User>, IUserRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Retrieves all users, eagerly loading their associated department.
    /// </summary>
    /// <returns>A collection of users including their departments.</returns>
    public override async Task<IEnumerable<User>> GetAllAsync() =>
        await _dbSet.Include(u => u.Department).ToListAsync();

    /// <summary>
    /// Retrieves a specific user by their unique identifier, eagerly loading their department.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>The found user, or null if they do not exist.</returns>
    public override async Task<User?> GetByIdAsync(Guid id) =>
        await _dbSet.Include(u => u.Department).FirstOrDefaultAsync(u => u.Id == id);

    /// <inheritdoc />
    public async Task<User?> GetByEmailAsync(string email) =>
        await _dbSet.FirstOrDefaultAsync(u => u.Email == email);

    /// <inheritdoc />
    public async Task<IEnumerable<User>> GetByDepartmentAsync(Guid departmentId) =>
        await _dbSet.Where(u => u.DepartmentId == departmentId).ToListAsync();
}
