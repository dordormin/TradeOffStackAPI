namespace TradeOffStackAPI.Repositories.Interfaces;

/// <summary>
/// Generic interface defining the standard contract for basic CRUD operations.
/// Standardizes data access across all domain entities in the system.
/// </summary>
/// <typeparam name="T">The type of the domain model entity (must be a class).</typeparam>
public interface IGenericRepository<T> where T : class
{
    /// <summary>
    /// Retrieves all entities of this type.
    /// </summary>
    /// <returns>An asynchronous collection containing all entities.</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Retrieves a specific entity by its unique identifier.
    /// </summary>
    /// <param name="id">The globally unique identifier (GUID) of the requested entity.</param>
    /// <returns>The found entity, or null if it does not exist.</returns>
    Task<T?> GetByIdAsync(Guid id);

    /// <summary>
    /// Checks if an entity exists in the database without fully loading it into memory.
    /// </summary>
    /// <param name="id">The identifier of the entity to verify.</param>
    /// <returns>True if the entity exists, otherwise false.</returns>
    Task<bool> ExistsAsync(Guid id);

    /// <summary>
    /// Adds a new entity to the database.
    /// </summary>
    /// <param name="entity">The complete entity object to insert.</param>
    /// <returns>True if the insertion and save were successful, otherwise false.</returns>
    Task<bool> AddAsync(T entity);

    /// <summary>
    /// Updates the information of an existing entity.
    /// </summary>
    /// <param name="entity">The entity object containing the updated values.</param>
    /// <returns>True if the update was successful, otherwise false.</returns>
    Task<bool> UpdateAsync(T entity);

    /// <summary>
    /// Permanently deletes an entity from the database.
    /// </summary>
    /// <param name="id">The identifier of the entity to delete.</param>
    /// <returns>True if the deletion was successful, otherwise false if the entity was not found.</returns>
    Task<bool> DeleteAsync(Guid id);
}
