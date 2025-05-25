using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace GrillBot.Core.Metrics.CustomTelemetry;

public class TelemetryService(
    Meter _meter,
    IEnumerable<ICustomTelemetryBuilder> _customTelemetryBuilders
) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        InitializeInternalTelemetry();
        InitializeCustomTelemetry();

        return Task.CompletedTask;
    }

    private void InitializeInternalTelemetry()
    {
        _meter.CreateObservableGauge("grillbot_workingset", () => Process.GetCurrentProcess().WorkingSet64, description: "Current working set of the service");
    }

    private void InitializeCustomTelemetry()
    {
        foreach (var builder in _customTelemetryBuilders)
            builder.BuildCustomTelemetry(_meter);
    }
}
