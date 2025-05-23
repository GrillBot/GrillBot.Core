﻿using System.Text.Json.Nodes;

namespace GrillBot.Core.RabbitMQ.V2.Consumer;

public interface IRabbitMessageHandler
{
    string TopicName { get; }
    string QueueName { get; }

    Task<RabbitConsumptionResult> HandleAsync(JsonNode? message, Dictionary<string, string> headers);
    Task<RabbitConsumptionResult> HandleRawMessageAsync(string rawMessage, Dictionary<string, string> headers);

    bool HandleException(Exception exception);
}
