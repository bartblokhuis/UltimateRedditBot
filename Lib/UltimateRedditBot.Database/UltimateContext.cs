using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UltimateRedditBot.Domain.Models.Common;
using UltimateRedditBot.Domain.Models.Reddit;

namespace UltimateRedditBot.Database
{
    public class UltimateContext : DbContext
    {
        public UltimateContext(DbContextOptions options)
            : base(options)
        {
        }


        public DbSet<Post> Posts { get; set; }

        public DbSet<Subreddit> Subreddits { get; set; }

        public DbSet<GenericSettings> GenericSettings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Used to update auditable entities.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var changes = from e in ChangeTracker.Entries()
                          where e.State != EntityState.Unchanged
                          select e;

            foreach (var change in changes)
            {
                if ((change.State == EntityState.Modified || change.State == EntityState.Added) && HasUpdatedTime(change.Entity))
                {
                    var updateTime = change.Entity as IHasUpdatedDate;
                    updateTime.UpdatedAt = DateTime.Now;

                }

                if (change.State == EntityState.Added && HasCreationTime(change.Entity))
                {
                    var creationTime = change.Entity as IHasCreationDate;
                    creationTime.CreatedAt = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        #region Helpers

        private bool HasUpdatedTime<TEntity>(TEntity entity)
        {
            var entityType = entity.GetType();
            return typeof(IHasUpdatedDate).IsAssignableFrom(entityType);
        }

        private bool HasCreationTime<TEntity>(TEntity entity)
        {
            var entityType = entity.GetType();
            return typeof(IHasCreationDate).IsAssignableFrom(entityType);
        }

        #endregion

    }
}
