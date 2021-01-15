using Microsoft.EntityFrameworkCore;
using UltimateRedditBot.Database.Common;
using UltimateRedditBot.Domain.Models.Reddit;
using UltimateRedditBot.Domain.Models.Settings;

namespace UltimateRedditBot.Database
{
    public class UltimateDbContext : BaseUltimateDbContext
    {
        public UltimateDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Subreddit> Subreddits { get; set; }

        public DbSet<GenericSetting> GenericSettings { get; set; }
    }
}
