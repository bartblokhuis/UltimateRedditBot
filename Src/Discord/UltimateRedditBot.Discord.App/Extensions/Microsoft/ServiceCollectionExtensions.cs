using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using UltimateRedditBot.Discord.App.Discord;
using UltimateRedditBot.Discord.App.Discord.Commands;
using UltimateRedditBot.Discord.App.Discord.Modules;

namespace UltimateRedditBot.Discord.App.Extensions.Microsoft
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDiscord(this IServiceCollection services)
        {
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
                .AddSingleton<QueueModule>()
                .AddSingleton<SettingsModule>()
                .AddSingleton<SubredditModule>()
                .AddSingleton<SubscriptionModule>();

        }
    }
}
