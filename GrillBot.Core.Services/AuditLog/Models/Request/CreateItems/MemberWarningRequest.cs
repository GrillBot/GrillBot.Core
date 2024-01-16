namespace GrillBot.Core.Services.AuditLog.Models.Request.CreateItems;

public class MemberWarningRequest
{
    public string Reason { get; set; } = null!;
    public string TargetId { get; set; } = null!;
}
