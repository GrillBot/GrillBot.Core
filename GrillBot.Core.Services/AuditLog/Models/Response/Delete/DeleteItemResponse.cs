namespace GrillBot.Core.Services.AuditLog.Models.Response.Delete;

public class DeleteItemResponse
{
    public bool Exists { get; set; }
    public List<string> FilesToDelete { get; set; } = new();
}
