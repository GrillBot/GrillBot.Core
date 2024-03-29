﻿namespace GrillBot.Core.Services.AuditLog.Models.Response;

public class ArchivationResult
{
    public string Content { get; set; } = null!;
    public List<string> Files { get; set; } = new();
    public List<string> UserIds { get; set; } = new();
    public List<string> GuildIds { get; set; } = new();
    public List<string> ChannelIds { get; set; } = new();
    public List<Guid> Ids { get; set; } = new();
    public int ItemsCount { get; set; }
    public long TotalFilesSize { get; set; }
    public Dictionary<string, long> PerType { get; set; } = new();
    public Dictionary<string, long> PerMonths { get; set; } = new();
}
