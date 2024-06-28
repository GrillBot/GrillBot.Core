using GrillBot.Core.RabbitMQ.Consumer;
using GrillBot.Core.RabbitMQ.Publisher;
using Microsoft.Extensions.DependencyInjection;

namespace GrillBot.Core.RabbitMQ;

public static class RabbitMQExtensions
{
    private static readonly Type _legacyHandlerInterfaceType = typeof(IRabbitMQHandler);

    public static IServiceCollection AddRabbitMQ(this IServiceCollection services)
    {
        services
            .AddSingleton<RabbitMQConnectionFactory>()
            .AddHostedService<RabbitMQConsumerService>()
            .AddSingleton(provider => provider.GetRequiredService<RabbitMQConnectionFactory>().Create())
            .AddSingleton<IRabbitMQPublisher, RabbitMQPublisher>()
            .AddScoped<IRabbitPublisher, RabbitPublisher>();

        services
            .AddHealthChecks()
            .AddCheck<RabbitMQHealthCheck>("RabbitMQ");

        return services;
    }

    public static IServiceCollection AddRabbitConsumerHandler(this IServiceCollection services, Type handlerType)
    {
        if (handlerType.GetInterface(_legacyHandlerInterfaceType.Name) is null)
            throw new InvalidOperationException($"Handler {handlerType.Name} does not impelement {_legacyHandlerInterfaceType.Name} interface");

        services.AddScoped(typeof(IRabbitMQHandler), handlerType);
        return services;
    }

    public static IServiceCollection AddRabbitConsumerHandler<THandler>(this IServiceCollection services) where THandler : class, IRabbitMQHandler
        => AddRabbitConsumerHandler(services, typeof(THandler));
}
