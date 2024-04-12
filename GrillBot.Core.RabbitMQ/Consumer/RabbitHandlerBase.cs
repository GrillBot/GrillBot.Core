using GrillBot.Core.RabbitMQ.Attributes;
using GrillBot.Core.RabbitMQ.Models;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace GrillBot.Core.RabbitMQ.Consumer;

public abstract class RabbitHandlerBase<TPayload> : IRabbitHandler
{
    public Type PayloadType => typeof(TPayload);

    public string QueueName =>
        PayloadType.GetCustomAttribute<RabbitQueueAttribute>()?.QueueName ??
        throw new KeyNotFoundException("Unable to find queue name from payload.");

    protected ILogger Logger { get; }

    protected RabbitHandlerBase(ILoggerFactory loggerFactory)
    {
        Logger = loggerFactory.CreateLogger(GetType());
    }

    public async Task HandleEventAsync(object? payload, Dictionary<string, string> headers)
    {
        if (payload is not TPayload payloadData)
            return;

        var message = new RabbitMessage<TPayload>(QueueName, payloadData);
        foreach (var header in headers)
            message.Headers.Add(header.Key, header.Value);

        await HandleMessageAsync(message);
    }

    protected abstract Task HandleMessageAsync(RabbitMessage<TPayload> message);

    public virtual Task HandleUnknownEventAsync(string message, Dictionary<string, string> headers)
    {
        Logger.LogWarning("Message: {message}", message);
        foreach (var header in headers)
            Logger.LogWarning("Header: {Key} => {Value}", header.Key, header.Value);

        return Task.CompletedTask;
    }
}
