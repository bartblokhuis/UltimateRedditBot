using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using UltimateRedditBot.Discord.Database;

namespace UltimateRedditBot.Database
{
    public class UltimateDbContextFactory: IDesignTimeDbContextFactory<UltimateDbContext>
    {
        public UltimateDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("dataSettings.json")
                .Build();

            var dbContextBuilder = new DbContextOptionsBuilder<UltimateDbContext>();
            var connectionString = configuration["ConnectionString:DefaultConnection"];

            dbContextBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("UltimateRedditBot.Migrations"));

            return new UltimateDbContext(dbContextBuilder.Options);
        }
    }

    public class UltimateDiscorDbContextFactory : IDesignTimeDbContextFactory<UltimateDiscordDbContext>
    {
        public UltimateDiscordDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("dataSettings.json")
                .Build();

            var dbContextBuilder = new DbContextOptionsBuilder();
            var connectionString = configuration["ConnectionString:DiscordConnection"];

            dbContextBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("UltimateRedditBot.Migrations"));

            return new UltimateDiscordDbContext(dbContextBuilder.Options);
        }
    }
}
