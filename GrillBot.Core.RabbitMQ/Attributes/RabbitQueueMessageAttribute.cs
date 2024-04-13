namespace GrillBot.Core.RabbitMQ.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class RabbitQueueMessageAttribute : Attribute
{
    public string QueueName { get; }

    public RabbitQueueMessageAttribute(string queueName)
    {
        QueueName = queueName;
    }
}
