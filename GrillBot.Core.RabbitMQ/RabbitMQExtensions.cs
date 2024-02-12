using GrillBot.Core.RabbitMQ.Consumer;
using GrillBot.Core.RabbitMQ.Publisher;
using Microsoft.Extensions.DependencyInjection;

namespace GrillBot.Core.RabbitMQ;

public static class RabbitMQExtensions
{
    public static IServiceCollection AddRabbitMQ(this IServiceCollection services)
    {
        services
            .AddSingleton<RabbitMQConnectionFactory>()
            .AddHostedService<RabbitMQConsumerService>()
            .AddSingleton(provider => provider.GetRequiredService<RabbitMQConnectionFactory>().Create())
            .AddSingleton<IRabbitMQPublisher, RabbitMQPublisher>();

        return services;
    }

    public static IServiceCollection AddRabbitConsumerHandler<THandler>(this IServiceCollection services) where THandler : class, IRabbitMQHandler
        => services.AddScoped<IRabbitMQHandler, THandler>();
}
