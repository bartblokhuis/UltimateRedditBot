using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UltimateRedditBot.App.Extensions.Microsoft;
using UltimateRedditBot.App.Services.Subscriptions;
using UltimateRedditBot.Discord.App.Discord;
using UltimateRedditBot.Discord.App.Discord.Commands;
using UltimateRedditBot.Discord.App.Extensions.Microsoft;

namespace UltimateRedditBot.Discord.Console
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            services.AddUltimateServices(Configuration["ConnectionString:DefaultConnection"]);
            services.AddDiscord(Configuration["ConnectionString:DiscordConnection"]);

            var provider = services.BuildServiceProvider();

            //Start discord services.
            provider.GetRequiredService<StartDiscord>().StartAsync().Wait();
            provider.GetRequiredService<CommandHandler>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.ApplicationServices.GetRequiredService<RedditSubscriptionHostedService>()
                .StartAsync(new CancellationToken());
        }
    }
}
