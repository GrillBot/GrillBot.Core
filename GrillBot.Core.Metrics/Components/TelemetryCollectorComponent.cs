using System.Diagnostics.Metrics;

namespace GrillBot.Core.Metrics.Components;

public abstract class TelemetryCollectorComponent(
    string name,
    Dictionary<string, object?>? tags = null,
    string? description = null
)
{
    private readonly object _lock = new();

    protected string Name => name;
    protected Dictionary<string, object?> Tags => tags ?? [];
    protected string? Description => description;

    protected void WithLock(Action action)
    {
        lock (_lock)
        {
            action();
        }
    }

    protected TValue WithLock<TValue>(Func<TValue> func)
    {
        lock (_lock)
        {
            return func();
        }
    }

    public abstract Instrument CreateInstrument(Meter meter);
}
