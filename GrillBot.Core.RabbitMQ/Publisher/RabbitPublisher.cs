using GrillBot.Core.Managers.Performance;
using GrillBot.Core.RabbitMQ.Extensions;
using GrillBot.Core.RabbitMQ.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace GrillBot.Core.RabbitMQ.Publisher;

public class RabbitPublisher : IRabbitPublisher
{
    private readonly IConnection _connection;
    private readonly ICounterManager _counterManager;

    public RabbitPublisher(IConnection connection, ICounterManager counterManager)
    {
        _connection = connection;
        _counterManager = counterManager;
    }

    public async Task PublishAsync<TPayload>(RabbitMessage<TPayload> message)
    {
        using (_counterManager.Create($"RabbitMQ.{message.QueueName}.Producer"))
            await SendMessageWithRetryPolicy(message, 5);
    }

    public async Task PublishAsync<TPayload>(IEnumerable<RabbitMessage<TPayload>> messages)
    {
        foreach (var message in messages)
            await PublishAsync(message);
    }

    private async Task SendMessageWithRetryPolicy<TPayload>(RabbitMessage<TPayload> message, int maxRetry, int currentRetry = 0)
    {
        try
        {
            using var queue = _connection.InitializeQueueModel(message.QueueName, true, false, false);

            var payloadData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message.Payload, RabbitMQSettings._serializerOptions));

            var properties = queue.CreateBasicProperties();
            properties.Persistent = true;
            properties.Headers = message.Headers.ToDictionary(o => o.Key, o => (object)o.Value);

            queue.BasicPublish("", message.QueueName, true, properties, payloadData);
        }
        catch (global::RabbitMQ.Client.Exceptions.AlreadyClosedException) when (currentRetry < maxRetry)
        {
            await Task.Delay(1000);
            await SendMessageWithRetryPolicy(message, maxRetry, currentRetry + 1);
        }
    }


}
