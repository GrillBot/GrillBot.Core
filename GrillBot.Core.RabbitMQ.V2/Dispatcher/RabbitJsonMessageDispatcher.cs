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
    public override async Task<RabbitConsumptionResult> HandleMessageAsync(IRabbitMessageHandler handler, byte[] body)
    {
        try
        {
            var serializer = GetSerializer<IJsonRabbitMessageSerializer>();
            var jsonNode = await serializer.DeserializeToJsonObjectAsync(body);

            return await handler.HandleAsync(jsonNode);
        }
        catch (JsonException ex)
        {
            _logger.LogWarning(ex, "An error occured while JSON deserialization. Trying process message with raw data.");
            return await base.HandleMessageAsync(handler, body);
        }
    }
}
