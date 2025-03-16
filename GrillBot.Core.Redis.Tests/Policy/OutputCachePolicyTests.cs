using GrillBot.Core.Redis.Policy;

namespace GrillBot.Core.Redis.Tests.Policy;

[TestClass]
public class OutputCachePolicyTests
{
    [TestMethod]
    public void FullConfiguration()
    {
        var policy = new OutputCachePolicy()
            .WithName("Name")
            .WithCacheKeyPrefix("CacheKey")
            .WithExpiration(TimeSpan.FromHours(1))
            .WithLocking(true)
            .WithVaryByHeaders("Header")
            .WithVaryByQuery("Query")
            .WithVaryByRouteValue("Route")
            .WithTags("Tag");

        Assert.AreEqual("Name", policy.Name);
        Assert.AreEqual("CacheKey", policy.CacheKeyPrefix);
        Assert.IsTrue(policy.Expiration.HasValue);
        Assert.AreEqual(1, policy.Expiration!.Value.Hours);
        Assert.IsTrue(policy.Locking.HasValue);
        Assert.IsTrue(policy.Locking!.Value);
        Assert.HasCount(1, policy.VaryByHeaders!);
        Assert.HasCount(1, policy.VaryByQuery!);
        Assert.HasCount(1, policy.VaryByRouteValue!);
        Assert.HasCount(1, policy.Tags!);
    }
}
