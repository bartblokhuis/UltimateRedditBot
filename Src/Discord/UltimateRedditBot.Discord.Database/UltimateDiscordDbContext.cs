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

        public DbSet<DiscordChannel> Guilds { get; set; }
    }
}
