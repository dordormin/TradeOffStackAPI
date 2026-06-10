using Microsoft.EntityFrameworkCore;
using TradeOffStackAPI.Data;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Repositories.Interfaces;

namespace TradeOffStackAPI.Repositories;

/// <summary>
/// Repository implementation for Department entities.
/// Overrides generic methods to include relational data (e.g., Users).
/// </summary>
public class DepartmentRepository : GenericRepository<Department, CoreDbContext>, IDepartmentRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DepartmentRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public DepartmentRepository(CoreDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Retrieves all departments, eagerly loading their associated users.
    /// </summary>
    /// <returns>A collection of departments including their users.</returns>
    public override async Task<IEnumerable<Department>> GetAllAsync() =>
        await _dbSet.Include(d => d.Users).ToListAsync();

    /// <summary>
    /// Retrieves a specific department by its unique identifier, eagerly loading its users.
    /// </summary>
    /// <param name="id">The unique identifier of the department.</param>
    /// <returns>The found department, or null if it does not exist.</returns>
    public override async Task<Department?> GetByIdAsync(Guid id) =>
        await _dbSet.Include(d => d.Users).FirstOrDefaultAsync(d => d.Id == id);
}
