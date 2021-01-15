using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace UltimateRedditBot.Database
{
    public class UltimateDbContextFactory: IDesignTimeDbContextFactory<UltimateDbContext>
    {
        public UltimateDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("dataSettings.json")
                .Build();

            var dbContextBuilder = new DbContextOptionsBuilder();
            var connectionString = configuration["ConnectionString:DefaultConnection"];

            dbContextBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("UltimateRedditBot.Migrations"));

            return new UltimateDbContext(dbContextBuilder.Options);
        }
    }
}
