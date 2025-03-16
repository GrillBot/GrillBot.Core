using Microsoft.AspNetCore.OutputCaching;

namespace GrillBot.Core.Redis.Policy;

public class OutputCachePolicy
{
    public string Name { get; private set; } = null!;
    public string? CacheKeyPrefix { get; private set; }
    public TimeSpan? Expiration { get; private set; }
    public bool? Locking { get; set; }
    public string[]? VaryByHeaders { get; set; }
    public string[]? VaryByQuery { get; set; }
    public string[]? VaryByRouteValue { get; set; }
    public string[]? Tags { get; set; }

    public OutputCachePolicy WithName(string name)
    {
        Name = name;
        return WithCacheKeyPrefix(name);
    }

    public OutputCachePolicy WithCacheKeyPrefix(string prefix)
    {
        CacheKeyPrefix = prefix;
        return this;
    }

    public OutputCachePolicy WithExpiration(TimeSpan expiration)
    {
        Expiration = expiration;
        return this;
    }

    public OutputCachePolicy WithLocking(bool locking = true)
    {
        Locking = locking;
        return this;
    }

    public OutputCachePolicy WithVaryByHeaders(params string[] headerNames)
    {
        VaryByHeaders = headerNames;
        return this;
    }

    public OutputCachePolicy WithVaryByQuery(params string[] queryNames)
    {
        VaryByQuery = queryNames;
        return this;
    }

    public OutputCachePolicy WithVaryByRouteValue(params string[] routeValueNames)
    {
        VaryByRouteValue = routeValueNames;
        return this;
    }

    public OutputCachePolicy WithTags(params string[] tags)
    {
        Tags = tags;
        return this;
    }

    public void ConfigurePolicy(OutputCachePolicyBuilder builder)
    {
        if (!string.IsNullOrEmpty(CacheKeyPrefix))
            builder.SetCacheKeyPrefix(CacheKeyPrefix);

        if (Expiration is not null)
            builder.Expire(Expiration.Value);

        if (Locking is not null)
            builder.SetLocking(Locking.Value);

        if (VaryByHeaders?.Length > 0)
            builder.SetVaryByHeader(VaryByHeaders);

        if (VaryByQuery?.Length > 0)
            builder.SetVaryByQuery(VaryByQuery);

        if (VaryByRouteValue?.Length > 0)
            builder.SetVaryByRouteValue(VaryByRouteValue);

        if (Tags?.Length > 0)
            builder.Tag(Tags);
    }
}
