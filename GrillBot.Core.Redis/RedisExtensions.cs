using GrillBot.Core.Redis.Policy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

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

        services.AddScoped(provider =>
        {
            var config = provider.GetRequiredService<IConfiguration>().GetSection("Redis")!;
            return ConnectionMultiplexer.Connect(CreateRedisOptions(config));
        });

        services.AddScoped(provider => provider.GetRequiredService<ConnectionMultiplexer>().GetDatabase());

        services.AddScoped(provider =>
        {
            var config = provider.GetRequiredService<IConfiguration>().GetSection("Redis")!;
            var connection = provider.GetRequiredService<ConnectionMultiplexer>();

            return connection.GetServer(config["Endpoint"]!);
        });

        return services.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = redisConfig["Endpoint"]!;
            opt.ConfigurationOptions = CreateRedisOptions(redisConfig);
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
            opt.ConfigurationOptions = CreateRedisOptions(redisConfig);
        });
    }

    private static ConfigurationOptions CreateRedisOptions(IConfigurationSection redisConfig)
    {
        return new()
        {
            AbortOnConnectFail = true,
            EndPoints = { redisConfig["Endpoint"]! },
            Password = redisConfig["Password"]!
        };
    }
}
