using GrillBot.Core.Redis.Policy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GrillBot.Core.Redis;

public static class RedisExtensions
{
    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddRedisDistributedCache(configuration)
            .AddRedisOutputCache(configuration);
    }

    public static IServiceCollection AddRedisDistributedCache(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConfig = configuration.GetSection("Redis");
        if (!redisConfig.Exists())
            return services;

        return services.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = redisConfig["Endpoint"]!;
            opt.ConfigurationOptions = new()
            {
                AbortOnConnectFail = true,
                EndPoints = { redisConfig["Endpoint"]! },
                Password = redisConfig["Password"]!
            };
        });
    }

    public static IServiceCollection AddRedisOutputCache(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddOutputCache(opt =>
        {
            var policies = opt.ApplicationServices.GetServices<OutputCachePolicy>()
                .Where(o => !string.IsNullOrEmpty(o.Name));

            foreach (var policy in policies)
                opt.AddPolicy(policy.Name, policy.ConfigurePolicy);

            opt.UseCaseSensitivePaths = false;
        });

        var redisConfig = configuration.GetSection("Redis");
        if (!redisConfig.Exists())
            return services; // Default output cache using in-memory.

        return services.AddStackExchangeRedisOutputCache(opt =>
        {
            opt.Configuration = redisConfig["Endpoint"]!;
            opt.ConfigurationOptions = new()
            {
                AbortOnConnectFail = true,
                EndPoints = { redisConfig["Endpoint"]! },
                Password = redisConfig["Password"]!
            };
        });
    }
}
