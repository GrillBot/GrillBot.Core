using System.Diagnostics.Metrics;

namespace GrillBot.Core.Metrics.Components;

public class TelemetryGaugeContainer(string name, string description) : TelemetryCollectorComponent(name, null, description)
{
    private readonly Dictionary<string, TelemetryGauge> _gauges = [];
    private readonly object _lock = new();

    public void Set(string name, long value, Dictionary<string, object?>? tags = null, string? description = null)
    {
        lock (_lock)
        {
            if (!_gauges.TryGetValue(name, out var gauge))
            {
                gauge = new(name, tags, description);
                _gauges.Add(name, gauge);
            }

            gauge.Set(value);
        }
    }

    public IEnumerable<Measurement<long>> Get()
    {
        lock (_lock)
        {
            return [.. _gauges.Values.Select(o => o.Get())];
        }
    }

    public override Instrument CreateInstrument(Meter meter)
        => meter.CreateObservableGauge(Name, Get, description: Description);
}
