using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UltimateRedditBot.App.Services.Events;
using UltimateRedditBot.App.Services.Queue;
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
            services.AddDbContext<UltimateDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddHttpClient();
            services.AddSingleton<IRedditApiService, RedditApiService>();
            services.AddSingleton<IQueueManager, QueueManager>();
            services.AddSingleton<IQueueService, QueueService>();
            services.AddSingleton<IGenericSettingService, GenericSettingService>();
            services.AddSingleton<IEventPublisher, EventPublisher>();
            services.AddSingleton<ISubscriptionService, SubscriptionService>();
            services.AddSingleton<ISubredditService, SubredditService>();
            services.AddScoped(typeof(IBaseRepository<,,>), typeof(BaseRepository<,,>));
            services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddAutoMapper(typeof(UltimateAutoMapperProfile));
        }

    }
}
