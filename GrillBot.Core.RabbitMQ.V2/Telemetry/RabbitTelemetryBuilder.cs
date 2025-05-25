using GrillBot.Core.Metrics.CustomTelemetry;
using System.Diagnostics.Metrics;

namespace GrillBot.Core.RabbitMQ.V2.Telemetry;

public class RabbitTelemetryBuilder(TelemetryCollector _collector) : ICustomTelemetryBuilder
{
    public void BuildCustomTelemetry(Meter meter)
    {
        meter.CreateObservableCounter("rabbit_published_messages", _collector.GetPublisherMeasurements, description: "Count of messages published to each queue.");
        meter.CreateObservableCounter("rabbit_consumed_messages", _collector.GetConsumerMeasurements, description: "Count of messages consumed from each queue.");
    }
}
