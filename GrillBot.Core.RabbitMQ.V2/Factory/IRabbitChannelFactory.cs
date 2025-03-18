using RabbitMQ.Client;

namespace GrillBot.Core.RabbitMQ.V2.Factory;

public interface IRabbitChannelFactory
{
    Task<IChannel> CreateChannelAsync(IConnection connection, string topicName, string? queueName = null);
}
