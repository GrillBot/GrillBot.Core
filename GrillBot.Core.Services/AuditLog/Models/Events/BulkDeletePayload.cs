namespace GrillBot.Core.Services.AuditLog.Models.Events;

public class BulkDeletePayload
{
    public List<Guid> Ids { get; set; } = [];

    public BulkDeletePayload()
    {
    }

    public BulkDeletePayload(List<Guid> ids)
    {
        Ids = ids;
    }

    public BulkDeletePayload(Guid id) : this([id])
    {
    }
}
