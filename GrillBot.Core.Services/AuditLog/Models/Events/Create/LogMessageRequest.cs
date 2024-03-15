﻿using Discord;

namespace GrillBot.Core.Services.AuditLog.Models.Events.Create;

public class LogMessageRequest
{
    public string Message { get; set; } = null!;
    public LogSeverity Severity { get; set; }
    public string SourceAppName { get; set; } = null!;
    public string Source { get; set; } = null!;
}
