﻿namespace GrillBot.Core.Services.AuditLog.Models.Response.Statistics;

public class InteractionStatistics
{
    public Dictionary<string, long> ByDate { get; set; } = [];
    public List<StatisticItem> Commands { get; set; } = [];
}
