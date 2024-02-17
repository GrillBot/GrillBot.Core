namespace GrillBot.Core.RabbitMQ;

public interface IPayload
{
    string QueueName { get; }
}
