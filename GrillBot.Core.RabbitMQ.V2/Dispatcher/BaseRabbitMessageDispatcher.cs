using GrillBot.Core.RabbitMQ.V2.Consumer;
using GrillBot.Core.RabbitMQ.V2.Serialization;

namespace GrillBot.Core.RabbitMQ.V2.Dispatcher;

public class BaseRabbitMessageDispatcher(
    IRabbitMessageSerializer _serializer
) : IRabbitMessageDispatcher
{
    public virtual async Task<RabbitConsumptionResult> HandleMessageAsync(IRabbitMessageHandler handler, byte[] body)
    {
        var rawMessage = await _serializer.DeserializeToStringAsync(body);
        return await handler.HandleRawMessageAsync(rawMessage);
    }

    protected TSerializer GetSerializer<TSerializer>() where TSerializer : IRabbitMessageSerializer
        => (TSerializer)_serializer;
}
