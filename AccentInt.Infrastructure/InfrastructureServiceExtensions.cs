using AccentInt.Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AccentInt.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, ILogger logger)
    {
        services.AddMemoryCache();
        services.AddScoped<ICache, Cache>();

        logger.LogInformation("Infrastructure registered");

        return services;
    }

}
