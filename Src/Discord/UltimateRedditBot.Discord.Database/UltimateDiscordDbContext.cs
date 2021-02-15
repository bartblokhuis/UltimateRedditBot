using Microsoft.EntityFrameworkCore;
using UltimateRedditBot.Database.Common;
using UltimateRedditBot.Discord.Domain.Models;

namespace UltimateRedditBot.Discord.Database
{
    public class UltimateDiscordDbContext : BaseUltimateDbContext
    {
        public UltimateDiscordDbContext(DbContextOptions<UltimateDiscordDbContext> options)
            : base(options)
        {
        }

        public DbSet<DiscordChannel> DiscordChannels { get; set; }

        public DbSet<Guild> Guilds { get; set; }

        public DbSet<GuildSettings> GuildSettings { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserSettings> UserSettings { get; set; }

        public DbSet<PostHistory> PostHistories { get; set; }

        public DbSet<BannedSubreddit> BannedSubreddits { get; set; }

        public DbSet<GuildMod> GuildMods { get; set; }

        public DbSet<TextChannel> TextChannels { get; set; }

        public DbSet<TextChannelSubscription> TextChannelSubscriptions { get; set; }

    }
}
