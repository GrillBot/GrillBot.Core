namespace GrillBot.Core.Services.AuditLog.Models.Events.Create;

public class CreateItemsPayload
{
    public List<LogRequest> Items { get; set; } = [];

    public CreateItemsPayload()
    {
    }

    public CreateItemsPayload(List<LogRequest> items)
    {
        Items = items;
    }

    public CreateItemsPayload(LogRequest item) : this([item])
    {
    }
}
