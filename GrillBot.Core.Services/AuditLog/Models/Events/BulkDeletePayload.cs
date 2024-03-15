using GrillBot.Core.RabbitMQ;

namespace GrillBot.Core.Services.AuditLog.Models.Events;

public class BulkDeletePayload : IPayload
{
    public string QueueName => "audit:bulk_delete";

    public List<Guid> Ids { get; set; } = new();

    public BulkDeletePayload()
    {
    }

    public BulkDeletePayload(List<Guid> ids)
    {
        Ids = ids;
    }
}
