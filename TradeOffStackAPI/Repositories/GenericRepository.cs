using Microsoft.EntityFrameworkCore;
using TradeOffStackAPI.Data;
using TradeOffStackAPI.Repositories.Interfaces;

namespace TradeOffStackAPI.Repositories;

/// <summary>
/// Generic implementation of the Repository pattern using Entity Framework Core.
/// Centralizes and factors out data access logic to reduce code duplication.
/// </summary>
/// <typeparam name="T">The EF Core entity type (must be a model class).</typeparam>
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    /// <summary>The injected database context.</summary>
    protected readonly AppDbContext _context;
    
    /// <summary>The typed DbSet corresponding to the current entity.</summary>
    protected readonly DbSet<T> _dbSet;

    /// <summary>
    /// Initializes a new instance of the generic repository.
    /// </summary>
    /// <param name="context">The Entity Framework database context.</param>
    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    /// <inheritdoc />
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    /// <inheritdoc />
    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    /// <inheritdoc />
    public virtual async Task<bool> ExistsAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            // Detach the entity to avoid conflicting tracking if it is modified later.
            _context.Entry(entity).State = EntityState.Detached;
            return true;
        }
        return false;
    }

    /// <inheritdoc />
    public virtual async Task<bool> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    /// <inheritdoc />
    public virtual async Task<bool> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    /// <inheritdoc />
    public virtual async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null) return false;
        
        _dbSet.Remove(entity);
        return await _context.SaveChangesAsync() > 0;
    }
}
