namespace GrillBot.Core.RabbitMQ.Models;

public class RabbitMessage<TPayload>
{
    public Dictionary<string, string> Headers { get; } = new();
    public string QueueName { get; }
    public TPayload Payload { get; }

    public RabbitMessage(string queueName, TPayload payload)
    {
        QueueName = queueName;
        Payload = payload;
    }
}
