using System.Diagnostics.Metrics;

namespace GrillBot.Core.Metrics.CustomTelemetry;

public interface ICustomTelemetryBuilder
{
    void BuildCustomTelemetry(Meter meter);
}
