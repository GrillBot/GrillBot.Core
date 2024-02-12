namespace GrillBot.Core.RabbitMQ.Consumer;

public interface IRabbitMQHandler
{
    string QueueName { get; }
    Type PayloadType { get; }

    Task HandleAsync(object? payload);
    Task HandleUnknownMessageAsync(string message);
}
