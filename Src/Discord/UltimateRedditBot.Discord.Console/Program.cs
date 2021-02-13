using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using UltimateRedditBot.Discord.Console;

namespace UltimateRedditBot.Discord
{
    internal class Program
    {
        //Startup
        private static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            try
            {
                Log.Information("Application starting up");
                MainAsync(args).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application didn't startup correctly");
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        private static Task MainAsync(string[] args)
        {
            return CreateHostBuilder(args).Build().RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureAppConfiguration(options =>
                {
                    options.AddJsonFile("appsettings.json");
                    options.AddJsonFile("dataSettings.json");
                })
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
        }
    }
}
