using RabbitMQ.Client;

namespace GrillBot.Core.RabbitMQ.V2.Factory;

public class RabbitChannelFactory : IRabbitChannelFactory
{
    public async Task<IChannel> CreateChannelAsync(IConnection connection, string topic, string? queue = null)
    {
        var channel = await connection.CreateChannelAsync();
        var deadLetterTopicName = $"{topic}.dead_letter";
        var deadLetterQueueName = $"{topic}.{queue}.dead_letter";

        await channel.ExchangeDeclareAsync(topic, ExchangeType.Topic, true); // Basic topic.
        await channel.ExchangeDeclareAsync(deadLetterTopicName, ExchangeType.Topic, true);

        if (string.IsNullOrEmpty(queue))
            return channel;

        var queueArgs = new Dictionary<string, object?>
        {
             { "x-dead-letter-exchange", deadLetterTopicName }
        };

        // Basic queue
        var queueName = $"{topic}.{queue}";
        await channel.QueueDeclareAsync(queueName, true, false, false, queueArgs);
        await channel.QueueBindAsync(queueName, topic, queue);

        // Dead letter queues
        await channel.QueueDeclareAsync(deadLetterQueueName, true, false, false);
        await channel.QueueBindAsync(deadLetterQueueName, deadLetterTopicName, deadLetterQueueName);

        return channel;
    }
}
