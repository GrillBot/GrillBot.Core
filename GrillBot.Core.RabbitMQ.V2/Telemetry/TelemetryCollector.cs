using System.Collections.Concurrent;
using System.Diagnostics.Metrics;

namespace GrillBot.Core.RabbitMQ.V2.Telemetry;

public class TelemetryCollector
{
    private readonly ConcurrentDictionary<(string topic, string queue), int> _consumerCounter = [];
    private readonly ConcurrentDictionary<(string topic, string queue), int> _publisherCounter = [];

    private static readonly object _consumerLock = new();
    private static readonly object _publisherLock = new();

    public void IncrementConsumer(string topic, string queue)
    {
        lock (_consumerLock)
        {
            Increment(_consumerCounter, topic, queue);
        }
    }

    public void IncrementProducer(string topic, string queue)
    {
        lock (_publisherLock)
        {
            Increment(_publisherCounter, topic, queue);
        }
    }

    public IEnumerable<Measurement<int>> GetConsumerMeasurements()
    {
        lock (_consumerLock)
        {
            return [.. GetMeasurements(_consumerCounter)];
        }
    }

    public IEnumerable<Measurement<int>> GetPublisherMeasurements()
    {
        lock (_publisherLock)
        {
            return [.. GetMeasurements(_publisherCounter)];
        }
    }

    private static void Increment(ConcurrentDictionary<(string topic, string queue), int> counter, string topic, string queue)
    {
        var key = (topic, queue);

        if (!counter.ContainsKey(key))
            counter.TryAdd(key, 0);
        counter[key]++;
    }

    private static IEnumerable<Measurement<int>> GetMeasurements(ConcurrentDictionary<(string queue, string topic), int> counter)
    {
        return counter
            .Select(o => new Measurement<int>(
                o.Value,
                KeyValuePair.Create<string, object?>("topic", o.Key.topic),
                KeyValuePair.Create<string, object?>("queue", o.Key.queue)
            ));
    }
}
