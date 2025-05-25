using GrillBot.Core.RabbitMQ.V2.Factory;
using GrillBot.Core.RabbitMQ.V2.Messages;
using GrillBot.Core.RabbitMQ.V2.Serialization;
using GrillBot.Core.RabbitMQ.V2.Telemetry;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace GrillBot.Core.RabbitMQ.V2.Publisher;

public class RabbitPublisher(
    IRabbitConnectionFactory _connectionFactory,
    IRabbitMessageSerializer _serializer,
    ILogger<RabbitPublisher> _logger,
    IRabbitChannelFactory _channelFactory,
    TelemetryCollector _telemetry
) : IRabbitPublisher
{
    private const int MAX_RETRIES = 5;

    public Task PublishAsync<T>(string topic, string queue, T data, Dictionary<string, string?>? headers = null)
        => PublishAsync(topic, queue, [data], headers);

    public async Task PublishAsync<T>(string topic, string queue, List<T> data, Dictionary<string, string?>? headers = null)
    {
        if (data.Count == 0)
            return;

        var properties = new BasicProperties
        {
            Persistent = true,
            Headers = headers?.ToDictionary(o => o.Key, o => (object?)o.Value)
        };

        _logger.LogInformation("Publishing messages to the topic {Topic}. Count: {Count}, Queue: \"{Queue}\"", topic, data.Count, queue);
        foreach (var item in data)
        {
            var messageData = await _serializer.SerializeMessageAsync(item);
            await PublishAsync(topic, queue, properties, messageData);
        }
    }

    public Task PublishAsync<T>(T message, Dictionary<string, string?>? headers = null) where T : IRabbitMessage
        => PublishAsync(message.Topic, message.Queue, [message], headers);

    public async Task PublishAsync<T>(List<T> messages, Dictionary<string, string?>? headers = null) where T : IRabbitMessage
    {
        foreach (var group in messages.GroupBy(m => new { m.Topic, m.Queue }))
            await PublishAsync(group.Key.Topic, group.Key.Queue, group.ToList(), headers);
    }

    private async Task PublishAsync(string topic, string queue, BasicProperties properties, byte[] body, int retry = 0)
    {
        try
        {
            _telemetry.IncrementProducer(topic, queue);

            await using var connection = await _connectionFactory.CreateAsync();
            await using var channel = await _channelFactory.CreateChannelAsync(connection, topic, queue);

            await channel.BasicPublishAsync(topic, queue, true, properties, body);
        }
        catch (global::RabbitMQ.Client.Exceptions.AlreadyClosedException) when (retry < MAX_RETRIES)
        {
            await Task.Delay(1000);
            await PublishAsync(topic, queue, properties, body, retry + 1);
        }
    }
}
