namespace GrillBot.Core.Managers.Random;

public class RandomManager : IRandomManager
{
    private readonly object _locker = new();
    private Dictionary<string, System.Random> Generators { get; } = new();

    private System.Random GetOrCreate(string key)
    {
        lock (_locker)
        {
            if (!Generators.ContainsKey(key))
                Generators.Add(key, new System.Random());

            return Generators[key];
        }
    }

    public int GetNext(string key, int min, int max)
        => GetOrCreate(key).Next(min, max);

    public int GetNext(string key, int max)
        => GetOrCreate(key).Next(max);
}
