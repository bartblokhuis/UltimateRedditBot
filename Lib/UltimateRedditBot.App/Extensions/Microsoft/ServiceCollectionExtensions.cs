using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UltimateRedditBot.App.Services.Events;
using UltimateRedditBot.App.Services.Queue;
using UltimateRedditBot.App.Services.Subscriptions;
using UltimateRedditBot.Core.AutoMapper;
using UltimateRedditBot.Core.BaseRepository;
using UltimateRedditBot.Core.Services;
using UltimateRedditBot.Database;
using UltimateRedditBot.Infra.BaseRepository;
using UltimateRedditBot.Infra.Services;

namespace UltimateRedditBot.App.Extensions.Microsoft
{
    public static class ServiceCollectionExtensions
    {
        public static void AddUltimateServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<UltimateDbContext>(options => { options.UseSqlServer(connectionString); });

            services.AddHttpClient()
                .AddSingleton<IRedditApiService, RedditApiService>()
                .AddSingleton<IQueueManager, QueueManager>()
                .AddSingleton<IQueueService, QueueService>()
                .AddSingleton<IGenericSettingService, GenericSettingService>()
                .AddSingleton<IEventPublisher, EventPublisher>()
                .AddSingleton<ISubscriptionService, SubscriptionService>()
                .AddSingleton<IRedditSubscriptionService, RedditSubscriptionService>()
                .AddSingleton<ISubredditService, SubredditService>()
                .AddSingleton<IRedditSubscriptionService, RedditSubscriptionService>()
                .AddScoped(typeof(IBaseRepository<,,>), typeof(BaseRepository<,,>))
                .AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>))
                .AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>))
                .AddAutoMapper(typeof(UltimateAutoMapperProfile));

            services.AddSingleton<RedditSubscriptionHostedService>();
        }
    }
}
