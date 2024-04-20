namespace GrillBot.Core.RabbitMQ.Consumer;

public interface IRabbitMQHandler
{
    string QueueName { get; }
    Type PayloadType { get; }

    Task HandleAsync(object? payload, Dictionary<string, string> headers);
    Task HandleUnknownMessageAsync(string message, Dictionary<string, string> headers);
}
