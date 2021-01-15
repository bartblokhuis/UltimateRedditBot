using AutoMapper;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UltimateRedditBot.Discord.App.AutoMapper;
using UltimateRedditBot.Discord.App.Discord;
using UltimateRedditBot.Discord.App.Discord.Commands;
using UltimateRedditBot.Discord.App.Discord.Modules;
using UltimateRedditBot.Discord.App.Discord.Modules.DirectMessage;
using UltimateRedditBot.Discord.App.Discord.Modules.Guild;
using UltimateRedditBot.Discord.App.Discord.Modules.Shared;
using UltimateRedditBot.Discord.App.Services;
using UltimateRedditBot.Discord.Database;

namespace UltimateRedditBot.Discord.App.Extensions.Microsoft
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDiscord(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<UltimateDiscordDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(connectionString);
            });

            services.AddAutoMapper(typeof(DiscordAutoMapperProfile));
            services.AddSingleton<IGuildService, GuildService>();

            services.AddSingleton(new CommandService(new CommandServiceConfig
                {
                    DefaultRunMode = RunMode.Async
                }))
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
                {
                    LogLevel = LogSeverity.Debug,
                    MessageCacheSize = 100
                }))
                .AddSingleton<StartDiscord>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<HelpModule>()
                .AddSingleton<DirectMessageSettingsModule>()
                .AddSingleton<GuildQueueModule>()
                .AddSingleton<GuildSettingsModule>()
                .AddSingleton<SubredditModule>()
                .AddSingleton<DirectMessageSettingsModule>();

        }
    }
}
