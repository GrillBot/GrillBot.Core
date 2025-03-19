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
    private const int MAX_RETRIES = 5;

    public Task PublishAsync<T>(string topicName, T data, string queueName, Dictionary<string, string>? headers = null)
        => PublishAsync(topicName, [data], queueName, headers);

    public async Task PublishAsync<T>(string topicName, List<T> data, string queueName, Dictionary<string, string>? headers = null)
    {
        if (data.Count == 0)
            return;

        var properties = new BasicProperties
        {
            Persistent = true,
            Headers = headers?.ToDictionary(o => o.Key, o => (object?)o.Value)
        };

        _logger.LogInformation("Publishing messages to the topic {TopicName}. Count: {Count}, Queue: \"{QueueName}\"", topicName, data.Count, queueName);
        foreach (var item in data)
        {
            var messageData = await _serializer.SerializeMessageAsync(item);
            await PublishAsync(topicName, queueName, properties, messageData);
        }
    }

    private async Task PublishAsync(string topicName, string queueName, BasicProperties properties, byte[] body, int retry = 0)
    {
        try
        {
            await using var connection = await _connectionFactory.CreateAsync();
            await using var channel = await _channelFactory.CreateChannelAsync(connection, topicName, queueName);

            await channel.BasicPublishAsync(topicName, queueName, true, body);
        }
        catch (global::RabbitMQ.Client.Exceptions.AlreadyClosedException) when (retry < MAX_RETRIES)
        {
            await Task.Delay(1000);
            await PublishAsync(topicName, queueName, properties, body, retry + 1);
        }
    }
}
