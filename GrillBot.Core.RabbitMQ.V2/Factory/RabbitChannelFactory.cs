using RabbitMQ.Client;

namespace GrillBot.Core.RabbitMQ.V2.Factory;

public class RabbitChannelFactory : IRabbitChannelFactory
{
    public async Task<IChannel> CreateChannelAsync(IConnection connection, string topicName, string? queueName = null)
    {
        var channel = await connection.CreateChannelAsync();
        var deadLetterTopicName = $"{topicName}.dead_letter";
        var deadLetterQueueName = $"{queueName}.dead_letter";

        await channel.ExchangeDeclareAsync(topicName, ExchangeType.Topic, true); // Basic topic.
        await channel.ExchangeDeclareAsync(deadLetterTopicName, ExchangeType.Topic, true);

        if (string.IsNullOrEmpty(queueName))
            return channel;

        var queueArgs = new Dictionary<string, object?>
        {
             { "x-dead-letter-exchange", deadLetterTopicName }
        };

        // Basic queue
        await channel.QueueDeclareAsync(queueName, true, false, false, queueArgs);
        await channel.QueueBindAsync(queueName, topicName, queueName);

        // Dead letter queues
        await channel.QueueDeclareAsync(deadLetterQueueName, true, false, false);
        await channel.QueueBindAsync(deadLetterQueueName, deadLetterTopicName, deadLetterQueueName);

        return channel;
    }
}
