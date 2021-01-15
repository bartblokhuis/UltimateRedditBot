using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UltimateRedditBot.Database;

namespace UltimateRedditBot.Infra.BaseRepository
{
    public interface IBaseRepository<TEntity> : IBaseRepository<TEntity, int>
    {

    }

    public interface IBaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey, UltimateDbContext>
    {

    }

    public interface IBaseRepository<TEntity, TKey, TDbContext>
    {
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task InsertAsync(TEntity entity);

        Task InsertAsync(IEnumerable<TEntity> entity);

        Task UpdateAsync(TEntity entity);

        Task UpdateRangeAsync(IEnumerable<TEntity> entities);

        Task DeleteAsync(TEntity entity);

        Task DeleteAsync(IEnumerable<TEntity> entities);

        Task DeleteAsync(TKey id);

        Task<TEntity> GetByIdAsync(TKey id);

        int Count();
    }
}
