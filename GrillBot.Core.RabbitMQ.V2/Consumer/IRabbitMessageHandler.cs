using System.Text.Json.Nodes;

namespace GrillBot.Core.RabbitMQ.V2.Consumer;

public interface IRabbitMessageHandler
{
    string TopicName { get; set; }
    string QueueName { get; set; }

    Task<RabbitConsumptionResult> HandleAsync(JsonNode? message);
    Task<RabbitConsumptionResult> HandleRawMessageAsync(string rawMessage);
}
