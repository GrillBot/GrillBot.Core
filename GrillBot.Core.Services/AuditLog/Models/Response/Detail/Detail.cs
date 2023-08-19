using GrillBot.Core.Services.AuditLog.Enums;

namespace GrillBot.Core.Services.AuditLog.Models.Response.Detail;

public class Detail
{
    public LogType Type { get; set; }
    public object? Data { get; set; }
}
