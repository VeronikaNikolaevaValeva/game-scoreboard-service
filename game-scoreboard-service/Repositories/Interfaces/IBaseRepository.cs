using game_scoreboard_service.Models;
using System.Linq.Expressions;

namespace game_scoreboard_service.Repositories.Interfaces
{
    /// <summary>
    /// A base repository which is inherited by other repositories and has basic CRUD operations.
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    public interface IBaseRepository<T> where T : class, IBaseModel
    {
        /// <summary>
        /// Gets a record by Partition Key.
        /// 
        /// If item with the given id is already being tracked, then return item without querying the database.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T?> GetByPartitionKeyAsync(string key);

        /// <summary>
        /// Gets a record by Partition Key.
        /// Also load related properties, if these are specified by includes.
        /// 
        /// Will always hit underlying DB connection
        /// </summary>
        /// <example>
        /// <code>
        ///     await _accountRoleRepository.GetByIdsAsync(ids, 
        ///         r => r.RolePropertyOne,
        ///         r => r.RolePropertyTwo,
        ///         r => r.RolePropertyThree);
        /// </code>
        /// </example>
        /// <param name="key"></param>
        /// <param name="includes"></param>
        /// <returns>An EntityModel.</returns>
        Task<T?> GetByPartitionKeyAsync(string key, params Expression<Func<T, object?>>[] includes);

        /// <summary>
        /// Gets all records from the table.
        /// </summary>
        /// <returns>A set of EntityModels.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Get an IEnumerable of entities from the database, where the Id was in the provided  Partition Keys.
        /// 
        /// Order of the provided ids enumerable does not need to be honoured, and returned elements may therefore be in any order.
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetByPartitionKeysAsync(params string[] keys);

        /// <summary>
        /// Get an IEnumerable of entities from the database, where the Partition Key was in the provided Partition Keys enumerable.
        /// Eager load any properties yielded by the includes methods
        /// 
        /// Order of the provided ids enumerable does not need to be honoured, and returned elements may therefore be in any order.
        /// 
        /// </summary>
        /// <example>
        /// <code>
        ///     await _playerScoreRepository.GetByIdsAsync(partitionKeys, 
        ///         p => p.EmailAddress,
        ///         r => r.Nickname);
        /// </code>
        /// </example>
        /// <param name="keys"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetByPartitionKeysAsync(IEnumerable<string> keys, params Expression<Func<T, object?>>[] includes);

        /// <summary>
        /// Adds a new record to the table.
        /// WARNING: if save changes is set to false, then do the same for other operations and then at the end use method SaveChanges() to save changes from all operations.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Added EntityModel.</returns>
        /// <remarks>will only Add the toplevel entity, this excludes relational entities.</remarks>
        Task<T?> AddAsync(T entity);

        /// <summary>
        /// Updates a record in the table.
        /// WARNING: if save changes is set to false, then do the same for other operations and then at the end use method SaveChanges() to save changes from all operations.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Updated EntityModel.</returns>
        /// <remarks>will only update the entity's table records, this excludes navigation properties for this update the foreign key instead.</remarks>
        Task<T?> UpdateAsync(T entity);
    }
}
