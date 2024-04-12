namespace GrillBot.Core.RabbitMQ.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class RabbitQueueAttribute : Attribute
{
    public string QueueName { get; }

    public RabbitQueueAttribute(string queueName)
    {
        QueueName = queueName;
    }
}
