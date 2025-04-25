using GrillBot.Core.Infrastructure.Auth;
using GrillBot.Core.RabbitMQ.V2.Messages;
using GrillBot.Core.RabbitMQ.V2.Serialization.Json;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace GrillBot.Core.RabbitMQ.V2.Consumer;

public abstract class RabbitMessageHandlerBase<TMessage>(
    ILoggerFactory _loggerFactory
) : IRabbitMessageHandler where TMessage : class, IRabbitMessage, new()
{
    public virtual string TopicName => new TMessage().Topic;
    public virtual string QueueName => new TMessage().Queue;

    protected ILogger Logger
        => _loggerFactory.CreateLogger(GetType());

    public async Task<RabbitConsumptionResult> HandleAsync(JsonNode? message, Dictionary<string, string> headers)
    {
        try
        {
            var deserializedMessage = message?.Deserialize<TMessage>(JsonRabbitMessageSerializer.SerializerOptions);
            if (deserializedMessage is null)
                return RabbitConsumptionResult.Reject;

            var currentUser = new CurrentUserProvider(headers);
            return await HandleInternalAsync(deserializedMessage, currentUser, headers);
        }
        catch (JsonException ex)
        {
            Logger.LogError(ex, "An error occured while deserializing message.");
            return RabbitConsumptionResult.Reject;
        }
    }

    protected abstract Task<RabbitConsumptionResult> HandleInternalAsync(
        TMessage message,
        ICurrentUserProvider currentUser,
        Dictionary<string, string> headers
    );

    public virtual bool HandleException(Exception exception) => false;

    public Task<RabbitConsumptionResult> HandleRawMessageAsync(string rawMessage, Dictionary<string, string> headers)
    {
        Logger.LogWarning("{Message}", rawMessage);
        foreach (var header in headers)
            Logger.LogWarning("Header({Key}): {Value}", header.Key, header.Value);

        return Task.FromResult(RabbitConsumptionResult.Success);
    }
}
