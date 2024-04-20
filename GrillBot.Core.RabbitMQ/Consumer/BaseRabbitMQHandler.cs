using Microsoft.Extensions.Logging;

namespace GrillBot.Core.RabbitMQ.Consumer;

public abstract class BaseRabbitMQHandler<TPayload> : IRabbitMQHandler
{
    public abstract string QueueName { get; }
    public Type PayloadType => typeof(TPayload);

    private ILogger Logger { get; }

    protected BaseRabbitMQHandler(ILoggerFactory loggerFactory)
    {
        Logger = loggerFactory.CreateLogger(GetType());
    }

    public async Task HandleAsync(object? payload, Dictionary<string, string> headers)
    {
        if (payload is not TPayload payloadData)
            return;

        await HandleInternalAsync(payloadData, headers);
    }

    protected abstract Task HandleInternalAsync(TPayload payload, Dictionary<string, string> headers);

    public virtual Task HandleUnknownMessageAsync(string message, Dictionary<string, string> headers)
    {
        Logger.LogWarning("{message}", message);
        foreach (var header in headers)
            Logger.LogWarning("Header({Key}): {Value}", header.Key, header.Value);
        return Task.CompletedTask;
    }
}
