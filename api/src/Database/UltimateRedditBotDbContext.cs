using Database.Configuration;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class UltimateRedditBotDbContext : DbContext
    {
        public UltimateRedditBotDbContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<BannedSubreddit> BannedSubreddits { get; set; }
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<GuildAdmin> GuildAdmins { get; set; }
        public DbSet<GuildChannel> GuildChannels { get; set; }
        public DbSet<GuildSetting> GuildSettings { get; set; }
        public DbSet<Subreddit> Subreddits { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostHistory> PostHistories { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<ChannelSubscription> ChannelSubscriptions { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GuildConfiguration());
            modelBuilder.ApplyConfiguration(new BannedSubredditConfiguration());
            modelBuilder.ApplyConfiguration(new GuildAdminConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
