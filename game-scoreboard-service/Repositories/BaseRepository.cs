using game_scoreboard_service.DataContext;
using game_scoreboard_service.Models;
using game_scoreboard_service.Repositories.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Drawing;
using System.Linq.Expressions;
using System.Net;
using System.Xml.Linq;

namespace game_scoreboard_service.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : class, IBaseModel
    {
        protected readonly DatabaseContext _db;

        protected BaseRepository(DatabaseContext db)
        {
            _db = db;
        }

        //Compiled Queries
        private static readonly Func<DatabaseContext, IAsyncEnumerable<TEntity>> _getAllAsync =
            EF.CompileAsyncQuery((DatabaseContext context) =>
                context.Set<TEntity>());

        private static readonly Func<DatabaseContext, string[], IAsyncEnumerable<TEntity>> _findByPartitionKeysAsync =
            EF.CompileAsyncQuery((DatabaseContext context, string[] ids) =>
                context.Set<TEntity>().Where(x => ids.Contains(x.PartitionKey)));

        private static readonly Func<DatabaseContext, string, Task<TEntity?>> _findByPartitionKeyAsync =
            EF.CompileAsyncQuery((DatabaseContext context, string key) =>
                context.Set<TEntity>().FirstOrDefault(x => x.PartitionKey == key));

        private static async Task<List<T>> ToListAsync<T>(IAsyncEnumerable<T> items, CancellationToken cancellationToken = default)
        {
            var results = new List<T>();
            await foreach (var item in items.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                results.Add(item);
            }

            return results;
        }
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await ToListAsync(_getAllAsync(_db));
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<TEntity>> GetByPartitionKeysAsync(params string[] keys)
        {
            return await ToListAsync(_findByPartitionKeysAsync(_db, keys));
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<TEntity>> GetByPartitionKeysAsync(IEnumerable<string> keys, params Expression<Func<TEntity, object?>>[] includes)
        {
            IQueryable<TEntity> query = _db.Set<TEntity>();
            foreach (var includeFunc in includes)
            {
                query = query.Include(includeFunc);
            }
            return await query.Where(x => keys.Contains(x.PartitionKey)).ToListAsync();
        }

        /// <inheritdoc />
        public virtual async Task<TEntity?> GetByPartitionKeyAsync(string key)
        {
            return await _findByPartitionKeyAsync(_db, key);
        }

        /// <inheritdoc />
        public virtual async Task<TEntity?> GetByPartitionKeyAsync(string key, params Expression<Func<TEntity, object?>>[] includes)
        {
            IQueryable<TEntity> query = _db.Set<TEntity>();
            foreach (var includeFunc in includes)
            {
                query = query.Include(includeFunc);
            }
            return await query.FirstOrDefaultAsync(x => x.PartitionKey == key);
        }
        /// <inheritdoc />
        public virtual async Task<TEntity?> AddAsync(TEntity entity)
        {
            try
            {
                var dbEntry = _db.Entry(entity).State = EntityState.Added;
                await _db.SaveChangesAsync();

                return entity;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Any ConcurrencyException caught during updating records should be handled here (e.g. trying to update a deleted entry)
                return null;
            }
        }

        /// <inheritdoc />
        public virtual async Task<TEntity?> UpdateAsync(TEntity entity)
        {
            try
            {
                _db.Entry(entity).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return entity;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Update Database, if a ConcurrencyException occurs, return null
                return null;
            }

        }
        /// <inheritdoc />
        public virtual async Task<bool?> DeleteAsync(TEntity entity)
        {
            try
            {
                _db.Entry(entity).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Update Database, if a ConcurrencyException occurs, return null
                return false;
            }

        }
        /// <inheritdoc />
        public virtual async Task<bool?> DeleteByPartitionKeyAsync(string key)
        {
            try
            {
                var entity = await _findByPartitionKeyAsync(_db, key);
                _db.Entry(entity).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Update Database, if a ConcurrencyException occurs, return null
                return false;
            }

        }

    }

}
