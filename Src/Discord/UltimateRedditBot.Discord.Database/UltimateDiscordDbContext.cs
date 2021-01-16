using Microsoft.EntityFrameworkCore;
using UltimateRedditBot.Database.Common;
using UltimateRedditBot.Discord.Domain.Models;

namespace UltimateRedditBot.Discord.Database
{
    public class UltimateDiscordDbContext : BaseUltimateDbContext
    {
        public UltimateDiscordDbContext(DbContextOptions<UltimateDiscordDbContext> options)
            :base(options)
        {

        }

        public DbSet<DiscordChannel> DiscordChannels { get; set; }

        public DbSet<Guild> Guilds { get; set; }

        public DbSet<GuildSettings> GuildSettings { get; set; }

        public DbSet<User> Users { get; set; }
        
        public DbSet<UserSettings> UserSettings { get; set; }
    }
}
