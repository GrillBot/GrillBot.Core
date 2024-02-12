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

    public async Task HandleAsync(object? payload)
    {
        if (payload is not TPayload payloadData)
            return;

        await HandleInternalAsync(payloadData);
    }

    protected abstract Task HandleInternalAsync(TPayload payload);

    public virtual Task HandleUnknownMessageAsync(string message)
    {
        Logger.LogWarning("{message}", message);
        return Task.CompletedTask;
    }
}
