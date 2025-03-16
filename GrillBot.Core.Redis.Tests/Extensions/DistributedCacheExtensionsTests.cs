using GrillBot.Core.Redis.Extensions;
using GrillBot.Core.Tests.Shared.TestCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace GrillBot.Core.Redis.Tests.Extensions;

[TestClass]
public class DistributedCacheExtensionsTests
{
    private IDistributedCache? _cache;

    private const string CacheKey = "DistributedCacheTests";

    [TestInitialize]
    public void TestInitialize()
    {
        var options = new MemoryDistributedCacheOptions();
        _cache = new MemoryDistributedCache(Options.Create(options));
    }

    [TestMethod]
    public async Task WithoutExpiration()
    {
        var data = new FlattenClass();
        await _cache!.SetAsync(CacheKey, data, null);

        var item = await _cache!.GetAsync<FlattenClass>(CacheKey);
        Assert.IsNotNull(item);
    }

    [TestMethod]
    public async Task WithExpiration()
    {
        var data = new FlattenClass();
        await _cache!.SetAsync(CacheKey, data, TimeSpan.FromMilliseconds(10));

        await Task.Delay(TimeSpan.FromMilliseconds(50));
        var item = await _cache!.GetAsync<FlattenClass>(CacheKey);
        Assert.IsNull(item);
    }
}
