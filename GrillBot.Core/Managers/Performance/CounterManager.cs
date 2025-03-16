namespace GrillBot.Core.Managers.Performance;

public class CounterManager : ICounterManager
{
    private List<CounterItem> ActiveCounters { get; } = [];
    private Dictionary<string, CounterStats> Stats { get; } = [];
    private readonly object _lock = new();

    public CounterItem Create(string section)
    {
        lock (_lock)
        {
            var item = new CounterItem(this, section);

            ActiveCounters.Add(item);
            return item;
        }
    }

    public void Complete(CounterItem item)
    {
        lock (_lock)
        {
            ActiveCounters.RemoveAll(o => o.Section == item.Section && o.Id == item.Id);

            if (!Stats.ContainsKey(item.Section))
                Stats.Add(item.Section, new CounterStats { Section = item.Section });

            var now = DateTime.Now;
            Stats[item.Section].Increment((now - item.StartAt).TotalMilliseconds);
        }
    }

    public Dictionary<string, int> GetActiveCounters()
    {
        lock (_lock)
        {
            return ActiveCounters
                .GroupBy(o => o.Section)
                .Select(o => new { o.Key, Count = o.Count() })
                .OrderByDescending(o => o.Count)
                .ThenBy(o => o.Key)
                .ToDictionary(o => o.Key, o => o.Count);
        }
    }

    public List<CounterStats> GetStatistics()
    {
        lock (_lock)
        {
            return Stats.Values.Select(o => o.Clone()).ToList();
        }
    }
}
