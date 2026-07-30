
using Btech.Package.Cache;
using Btech.Package.Cache.Configurations;
using Microsoft.Extensions.DependencyInjection; 

namespace IQT_FSD_2026.Application;

public static class DependencyInjections
{
    public static IServiceCollection AddApplicationDependencyInjections(this IServiceCollection services)
    {
        var cacheConfigurations = new CacheConfigurations()
        {
            Provider = "Redis", //"InMemory"
            RedisConnection = "localhost:6379,allowAdmin=true,abortConnect=false",
            RedisInstanceName = "InstanceName_",
            RedisServiceName = "ServiceName",
            KeyPrefix = ""
        };
        services.AddCachingServices(cacheConfigurations);

        return services;
    }
}
