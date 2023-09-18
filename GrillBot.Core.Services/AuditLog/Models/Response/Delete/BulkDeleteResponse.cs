namespace GrillBot.Core.Services.AuditLog.Models.Response.Delete;

public class BulkDeleteResponse
{
    public List<DeleteItemResponse> Items { get; set; } = new();
}
