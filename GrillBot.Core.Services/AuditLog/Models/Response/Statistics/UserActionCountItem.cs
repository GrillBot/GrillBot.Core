﻿namespace GrillBot.Core.Services.AuditLog.Models.Response.Statistics;

public class UserActionCountItem
{
    public string UserId { get; set; } = null!;
    public string Action { get; set; } = null!;
    public long Count { get; set; }
}
