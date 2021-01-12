using Microsoft.EntityFrameworkCore;
using UltimateRedditBot.Database;
using UltimateRedditBot.Discord.Domain.Models;

namespace UltimateRedditBot.Discord.Database
{
    public class UltimateDiscordDbContext : UltimateContext
    {
        public UltimateDiscordDbContext(DbContextOptions options)
            :base(options)
        {

        }

        public DbSet<DiscordChannel> DiscordChannels { get; set; }

        public DbSet<DiscordChannel> Guilds { get; set; }
    }
}
