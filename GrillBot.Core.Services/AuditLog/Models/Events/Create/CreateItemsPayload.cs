using GrillBot.Core.RabbitMQ;

namespace GrillBot.Core.Services.AuditLog.Models.Events.Create;

public class CreateItemsPayload : IPayload
{
    public string QueueName => "audit:create_items";

    public List<LogRequest> Items { get; set; } = new();

    public CreateItemsPayload()
    {
    }

    public CreateItemsPayload(List<LogRequest> items)
    {
        Items = items;
    }

    public CreateItemsPayload(LogRequest item) : this(new List<LogRequest> { item })
    {
    }
}
