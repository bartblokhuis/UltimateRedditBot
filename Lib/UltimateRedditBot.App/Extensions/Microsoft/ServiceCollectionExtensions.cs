using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddScoped(typeof(IBaseRepository<,,>), typeof(BaseRepository<,,>));
            services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddAutoMapper(typeof(UltimateAutoMapperProfile));


        }

    }
}
