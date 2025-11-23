using AccentInt.Application;
using AccentInt.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;

namespace AccentInt.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, ILogger logger)
    {
        services.AddMemoryCache();
        services.AddScoped<ICache, Cache>();

        services.AddHttpClient<IClient, DateNagerClient>(config =>
        {
            config.BaseAddress = new Uri("https://date.nager.at/api/v3/");
            config.Timeout = TimeSpan.FromSeconds(20);
        })
        .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(3)));

        services.AddScoped<IHolidayService, HolidaysService>();

        logger.LogInformation("Infrastructure registered");

        return services;
    }
}
