using GrillBot.Core.RabbitMQ.Consumer;
using GrillBot.Core.RabbitMQ.Publisher;
using Microsoft.Extensions.DependencyInjection;

namespace GrillBot.Core.RabbitMQ;

public static class RabbitMQExtensions
{
    private static readonly Type _legacyHandlerInterfaceType = typeof(IRabbitMQHandler);
    private static readonly Type _handlerInterface = typeof(IRabbitHandler);

    [Obsolete("Use AddRabbit instead")]
    public static IServiceCollection AddRabbitMQ(this IServiceCollection services)
    {
        services
            .AddSingleton<RabbitMQConnectionFactory>()
            .AddHostedService<RabbitMQConsumerService>()
            .AddSingleton(provider => provider.GetRequiredService<RabbitMQConnectionFactory>().Create())
            .AddSingleton<IRabbitMQPublisher, RabbitMQPublisher>();

        services
            .AddHealthChecks()
            .AddCheck<RabbitMQHealthCheck>("RabbitMQ");

        return services;
    }

    public static IServiceCollection AddRabbit(this IServiceCollection services)
    {
        services
            .AddSingleton<RabbitMQConnectionFactory>()
            .AddSingleton(provider => provider.GetRequiredService<RabbitMQConnectionFactory>().Create())
            .AddSingleton<IRabbitPublisher, RabbitPublisher>();

        services
            .AddHostedService<RabbitConsumerService>();

        services
            .AddHealthChecks()
            .AddCheck<RabbitMQHealthCheck>("Rabbit");

        return services;
    }

    [Obsolete("Use AddRabbitConsumer instead.")]
    public static IServiceCollection AddRabbitConsumerHandler(this IServiceCollection services, Type handlerType)
    {
        if (handlerType.GetInterface(_legacyHandlerInterfaceType.Name) is null)
            throw new InvalidOperationException($"Handler {handlerType.Name} does not impelement {_legacyHandlerInterfaceType.Name} interface");

        services.AddScoped(typeof(IRabbitMQHandler), handlerType);
        return services;
    }

    [Obsolete("Use AddRabbitConsumer instead")]
    public static IServiceCollection AddRabbitConsumerHandler<THandler>(this IServiceCollection services) where THandler : class, IRabbitMQHandler
        => AddRabbitConsumerHandler(services, typeof(THandler));

    public static IServiceCollection AddRabbitConsumer(this IServiceCollection services, Type handlerType)
    {
        if (handlerType.GetInterface(_handlerInterface.Name) is null)
            throw new InvalidOperationException($"Handler {handlerType.Name} does not impelement {_handlerInterface.Name} interface");

        return services.AddScoped(_handlerInterface, handlerType);
    }

    public static IServiceCollection AddRabbitConsumer<THandler>(this IServiceCollection services) where THandler : class, IRabbitHandler
        => AddRabbitConsumer(services, typeof(THandler));
}
