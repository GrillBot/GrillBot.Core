using GrillBot.Core.RabbitMQ.V2.Consumer;
using GrillBot.Core.RabbitMQ.V2.Serialization;
using GrillBot.Core.RabbitMQ.V2.Serialization.Json;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GrillBot.Core.RabbitMQ.V2.Dispatcher;

public class RabbitJsonMessageDispatcher(
    IRabbitMessageSerializer serializer,
    ILogger<RabbitJsonMessageDispatcher> _logger
) : BaseRabbitMessageDispatcher(serializer)
{
    public override async Task<RabbitConsumptionResult> HandleMessageAsync(IRabbitMessageHandler handler, byte[] body, Dictionary<string, string> headers)
    {
        try
        {
            var jsonSerializer = GetSerializer<IJsonRabbitMessageSerializer>();
            var jsonNode = await jsonSerializer.DeserializeToJsonObjectAsync(body);

            return await handler.HandleAsync(jsonNode, headers);
        }
        catch (JsonException ex)
        {
            _logger.LogWarning(ex, "An error occured while JSON deserialization. Trying process message with raw data.");
            return await base.HandleMessageAsync(handler, body, headers);
        }
    }
}
