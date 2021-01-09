using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UltimateRedditBot.Infra.BaseRepository
{
    public interface IBaseRepository<TEntity> : IBaseRepository<TEntity, int>
    {

    }

    public interface IBaseRepository<TEntity, in TKey>
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
