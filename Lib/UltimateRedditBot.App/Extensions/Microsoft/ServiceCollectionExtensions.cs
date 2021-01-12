using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using UltimateRedditBot.Core.AutoMapper;
using UltimateRedditBot.Core.Services;
using UltimateRedditBot.Infra.Services;

namespace UltimateRedditBot.App.Extensions.Microsoft
{
    public static class ServiceCollectionExtensions
    {
        public static void AddUltimateServices(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<IRedditApiService, RedditApiService>();
            services.AddAutoMapper(typeof(UltimateAutoMapperProfile));
        }

    }
}
