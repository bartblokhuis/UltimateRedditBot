using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UltimateRedditBot.Domain.Models.Common;

namespace UltimateRedditBot.Database.Common
{
    public abstract class BaseUltimateDbContext : DbContext
    {
        protected BaseUltimateDbContext(DbContextOptions options)
            : base(options)
        {
        }

        /// <summary>
        ///     Used to update auditable entities.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var changes = from e in ChangeTracker.Entries()
                where e.State != EntityState.Unchanged
                select e;

            foreach (var change in changes)
            {
                if ((change.State == EntityState.Modified || change.State == EntityState.Added) &&
                    HasUpdatedTime(change.Entity) && change.Entity is IHasUpdatedDate updateTime)
                    updateTime.UpdatedAtUTC = DateTime.UtcNow;

                if (change.State != EntityState.Added || !HasCreationTime(change.Entity))
                    continue;

                if (change.Entity is IHasCreationDate creationTime)
                    creationTime.CreatedAtUTC = DateTime.UtcNow;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        #region Helpers

        private static bool HasUpdatedTime<TEntity>(TEntity entity)
        {
            var entityType = entity.GetType();
            return typeof(IHasUpdatedDate).IsAssignableFrom(entityType);
        }

        private static bool HasCreationTime<TEntity>(TEntity entity)
        {
            var entityType = entity.GetType();
            return typeof(IHasCreationDate).IsAssignableFrom(entityType);
        }

        #endregion
    }
}
