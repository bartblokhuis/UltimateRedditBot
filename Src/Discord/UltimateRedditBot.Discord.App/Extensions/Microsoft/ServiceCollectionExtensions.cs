using System.Collections.Generic;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using UltimateRedditBot.App.Services.Events;
using UltimateRedditBot.App.Services.Queue;
using UltimateRedditBot.App.Services.Subscriptions;
using UltimateRedditBot.Discord.App.AutoMapper;
using UltimateRedditBot.Discord.App.Discord;
using UltimateRedditBot.Discord.App.Discord.Commands;
using UltimateRedditBot.Discord.App.Discord.Modules.DirectMessage;
using UltimateRedditBot.Discord.App.Discord.Modules.Guild;
using UltimateRedditBot.Discord.App.Discord.Modules.Shared;
using UltimateRedditBot.Discord.App.Events;
using UltimateRedditBot.Discord.App.Services;
using UltimateRedditBot.Discord.App.Services.Guild;
using UltimateRedditBot.Discord.App.Services.Queue;
using UltimateRedditBot.Discord.App.Services.TextChannelService;
using UltimateRedditBot.Discord.App.Services.User;
using UltimateRedditBot.Discord.Database;

namespace UltimateRedditBot.Discord.App.Extensions.Microsoft
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDiscord(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<UltimateDiscordDbContext>(options => { options.UseSqlServer(connectionString); });

            services.AddAutoMapper(typeof(DiscordAutoMapperProfile))
                .AddScoped<IGuildService, GuildService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IPostHistoryService, PostHistoryService>()
                .AddScoped<ITextChannelService, TextChannelService>()
                .AddScoped<IBannedSubredditService, BannedSubredditService>()
                .AddScoped<IGuildModService, GuildModService>()
                .AddScoped<ITextChannelSubscriptionService, TextChannelSubscriptionService>();

            services.AddScoped<IConsumer<QueueItemPostReceived>, DiscordQueueItemReceived>();
            services.AddScoped<IConsumer<IEnumerable<SubscriptionPost>>, SubscriptionsPostReceived>();
            services.Replace(ServiceDescriptor.Singleton<IQueueService, DiscordQueueService>());

            services.AddSingleton(new CommandService(new CommandServiceConfig
                {
                    DefaultRunMode = RunMode.Async
                }))
                .AddSingleton(new DiscordShardedClient(new DiscordSocketConfig
                {
                    LogLevel = LogSeverity.Debug,
                    MessageCacheSize = 100
                }))
                .AddSingleton<StartDiscord>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<DiscordLoggingService>()
                .AddSingleton<HelpModule>()
                .AddSingleton<GuildQueueModule>()
                .AddSingleton<SharedQueueModule>()
                .AddSingleton<GuildSettingsModule>()
                .AddSingleton<SubredditModule>()
                .AddSingleton<DirectMessageSettingsModule>()
                .AddSingleton<CleanModule>()
                .AddSingleton<GuildModModule>()
                .AddSingleton<SubscribeModule>()
                .AddSingleton<BanSubredditModule>();
        }
    }
}
