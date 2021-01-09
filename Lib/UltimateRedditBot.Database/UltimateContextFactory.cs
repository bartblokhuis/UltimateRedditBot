using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace UltimateRedditBot.Database
{
    public class UltimateContextFactory : IDesignTimeDbContextFactory<UltimateContext>
    {
        public UltimateContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                 .AddJsonFile("datasettings.json")
                 .Build();

            var dbContextBuilder = new DbContextOptionsBuilder();
            var connectionString = configuration["ConnectionString:DefaultConnection"];

            dbContextBuilder.UseSqlServer(connectionString);

            return new UltimateContext(dbContextBuilder.Options);
        }
    }
}
