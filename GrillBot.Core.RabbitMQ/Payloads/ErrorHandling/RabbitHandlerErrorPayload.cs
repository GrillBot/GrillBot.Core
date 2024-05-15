using System.Reflection;

namespace GrillBot.Core.RabbitMQ.Payloads.ErrorHandling;

/// <summary>
/// Payload represents error handled in the queue message processing.
/// </summary>
public class RabbitHandlerErrorPayload : IPayload
{
    public string QueueName => "rabbit:errors";

    public string AssemblyName { get; set; } = null!;
    public string ReceiverQueueName { get; set; } = null!;
    public string FullException { get; set; } = null!;
    public string? Message { get; set; }
    public Dictionary<string, string> Headers { get; set; } = new();
    public string PayloadType { get; set; } = null!;
    public string HandlerType { get; set; } = null!;

    public RabbitHandlerErrorPayload()
    {
    }

    public RabbitHandlerErrorPayload(Exception ex, string receiverQueueName, string? message, Dictionary<string, string> headers, string payloadType, string handlerType)
    {
        AssemblyName = Assembly.GetEntryAssembly()!.GetName().Name!;
        ReceiverQueueName = receiverQueueName;
        Headers = headers;
        PayloadType = payloadType;
        HandlerType = handlerType;
        Message = message;
        FullException = ex.ToString();
    }
}
