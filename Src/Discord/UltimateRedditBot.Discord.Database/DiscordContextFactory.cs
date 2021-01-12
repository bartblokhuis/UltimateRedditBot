using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace UltimateRedditBot.Discord.Database
{
    public class DiscordContextFactory: IDesignTimeDbContextFactory<UltimateDiscordDbContext>
    {
        public UltimateDiscordDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("dataSettings.json")
                .Build();

            var dbContextBuilder = new DbContextOptionsBuilder();
            var connectionString = configuration["ConnectionString:DefaultConnection"];

            dbContextBuilder.UseSqlServer(connectionString);

            return new UltimateDiscordDbContext(dbContextBuilder.Options);
        }
    }
}
