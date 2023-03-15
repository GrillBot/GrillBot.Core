using System.Diagnostics;
using GrillBot.Core.Database;
using GrillBot.Core.Managers.Performance;
using GrillBot.Core.Services.Diagnostics.Models;

namespace GrillBot.Core.Services.Diagnostics;

public class DiagnosticsProvider : IDiagnosticsProvider
{
    private DiagnosticsManager Manager { get; }
    private IStatisticsProvider? StatisticsProvider { get; }
    private ICounterManager CounterManager { get; }

    public DiagnosticsProvider(DiagnosticsManager manager, ICounterManager counterManager)
    {
        Manager = manager;
        CounterManager = counterManager;
    }

    public DiagnosticsProvider(DiagnosticsManager manager, IStatisticsProvider statisticsProvider, ICounterManager counterManager) : this(manager, counterManager)
    {
        StatisticsProvider = statisticsProvider;
    }

    public async Task<DiagnosticInfo> GetInfoAsync()
    {
        var process = Process.GetCurrentProcess();
        var databaseStatistics = StatisticsProvider == null ? null : await StatisticsProvider.GetTableStatisticsAsync();
        var operationStats = CounterManager.GetStatistics();

        return new DiagnosticInfo
        {
            Endpoints = Manager.Statistics,
            Uptime = Convert.ToInt64((DateTime.Now - process.StartTime).TotalMilliseconds),
            CpuTime = Convert.ToInt64(process.TotalProcessorTime.TotalMilliseconds),
            MeasuredFrom = process.StartTime,
            RequestsCount = Manager.Statistics.Sum(o => o.Count),
            UsedMemory = process.WorkingSet64,
            DatabaseStatistics = databaseStatistics,
            Operations = OperationCounterConverter.ComputeTree(operationStats)
        };
    }
}
