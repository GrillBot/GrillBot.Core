namespace GrillBot.Core.Services.AuditLog.Models.Request.CreateItems;

public class UserLeftRequest : UserJoinedRequest
{
    public string UserId { get; set; } = null!;
}
