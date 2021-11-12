using Api.Extensions;
using Api.Helpers;
using Api.Options;
using Core.Consts;
using Core.Extensions;
using Database;
using Domain.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
            c.OperationFilter<SwaggerHeaderParameter>();
        });

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = ApiKeyAuthenticationOptions.DefaultScheme;
            options.DefaultChallengeScheme = ApiKeyAuthenticationOptions.DefaultScheme;
        })
        .AddApiKeySupport(options => { });

        var connectionString = Configuration.GetConnectionString("Default");
        services.AddDbContext<UltimateRedditBotDbContext>(options =>
        {
            options.UseSqlServer(connectionString, e => e.MigrationsAssembly(typeof(UltimateRedditBotDbContext).Assembly.FullName));
        });

        using var scope = services.BuildServiceProvider().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<UltimateRedditBotDbContext>();

        var limitSettings = new LimitSettings()
        {
            DefaultMaxPlaylists = Configuration.GetValue<int>(SettingKeys.DefaultMaxPlaylists),
            DefaultMaxItemsInQueue = Configuration.GetValue<int>(SettingKeys.DefaultMaxItemsInQueue),
            DefaultMaxSubscriptions = Configuration.GetValue<int>(SettingKeys.DefaultMaxSubscriptions),
        };
        services.AddSingleton(limitSettings);

        services.AddCors();

        //Apply last changes
        dbContext.Database.Migrate();

        services.AddCore();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));

        app.UseHttpsRedirection();

        app.UseRouting();

        // global cors policy
        app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true) // allow any origin
            .AllowCredentials()); // allow credentials

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
