using System.Diagnostics;
using GrillBot.Core.Database;
using GrillBot.Core.Services.Diagnostics.Models;

namespace GrillBot.Core.Services.Diagnostics;

public class DiagnosticsProvider : IDiagnosticsProvider
{
    private DiagnosticsManager Manager { get; }
    private IStatisticsProvider? StatisticsProvider { get; }

    public DiagnosticsProvider(DiagnosticsManager manager)
    {
        Manager = manager;
    }

    public DiagnosticsProvider(DiagnosticsManager manager, IStatisticsProvider statisticsProvider) : this(manager)
    {
        StatisticsProvider = statisticsProvider;
    }

    public async Task<DiagnosticInfo> GetInfoAsync()
    {
        var process = Process.GetCurrentProcess();
        var databaseStatistics = StatisticsProvider == null ? null : await StatisticsProvider.GetTableStatisticsAsync();

        return new DiagnosticInfo
        {
            Endpoints = Manager.Statistics,
            Uptime = Convert.ToInt64((DateTime.Now - process.StartTime).TotalMilliseconds),
            CpuTime = Convert.ToInt64(process.TotalProcessorTime.TotalMilliseconds),
            MeasuredFrom = process.StartTime,
            RequestsCount = Manager.Statistics.Sum(o => o.Count),
            UsedMemory = process.WorkingSet64,
            DatabaseStatistics = databaseStatistics
        };
    }
}
