using GrillBot.Core.RabbitMQ.V2.Factory;
using GrillBot.Core.RabbitMQ.V2.Serialization;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace GrillBot.Core.RabbitMQ.V2.Publisher;

public class RabbitPublisher(
    IRabbitConnectionFactory _connectionFactory,
    IRabbitMessageSerializer _serializer,
    ILogger<RabbitPublisher> _logger,
    IRabbitChannelFactory _channelFactory
) : IRabbitPublisher
{
    public Task PublishAsync<T>(string topicName, T data, string? queueName = null)
        => PublishAsync(topicName, [data], queueName);

    public async Task PublishAsync<T>(string topicName, List<T> data, string? queueName = null)
    {
        await using var connection = await _connectionFactory.CreateAsync();
        await using var channel = await _channelFactory.CreateChannelAsync(connection, topicName, queueName);

        var properties = new BasicProperties
        {
            Persistent = true
        };

        _logger.LogInformation("Publishing messages to the topic {TopicName}. Count: {Count}, Queue: \"{QueueName}\"", topicName, data.Count, queueName);
        foreach (var item in data)
        {
            var messageData = await _serializer.SerializeMessageAsync(item);
            await channel.BasicPublishAsync(topicName, "", true, properties, messageData);
        }
    }
}
