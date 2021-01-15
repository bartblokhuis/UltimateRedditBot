using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UltimateRedditBot.Database.Common;
using UltimateRedditBot.Domain.Models.Common;
using UltimateRedditBot.Domain.Models.Reddit;
using UltimateRedditBot.Domain.Models.Settings;

namespace UltimateRedditBot.Database
{
    public abstract class UltimateContext : BaseUltimateDbContext
    {
        protected UltimateContext(DbContextOptions options)
            : base(options)
        {
        }


        public DbSet<Post> Posts { get; set; }

        public DbSet<Subreddit> Subreddits { get; set; }

        public DbSet<GenericSetting> GenericSettings { get; set; }

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
