using System.Net.Http.Json;
using GrillBot.Core.Managers.Performance;
using GrillBot.Core.Services.Graphics.Models.Chart;
using GrillBot.Core.Services.Graphics.Models.Diagnostics;
using GrillBot.Core.Services.Graphics.Models.Images;
using Newtonsoft.Json.Linq;

namespace GrillBot.Core.Services.Graphics;

public class GraphicsClient : RestServiceBase, IGraphicsClient
{
    public override string ServiceName => "Graphics";

    public GraphicsClient(IHttpClientFactory httpClientFactory, ICounterManager counterManager) : base(counterManager, httpClientFactory)
    {
    }

    public async Task<byte[]> CreateChartAsync(ChartRequestData request)
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.PostAsJsonAsync("chart", request, cancellationToken),
            (response, cancellationToken) => response.Content.ReadAsByteArrayAsync(cancellationToken: cancellationToken)!,
            timeout: System.Threading.Timeout.InfiniteTimeSpan
        );
    }

    public async Task<Metrics> GetMetricsAsync()
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync("metrics", cancellationToken),
            async (response, cancellationToken) =>
            {
                var json = JObject.Parse(await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken));
                return new Metrics
                {
                    Uptime = (long)Math.Ceiling(json["uptime"]!.Value<double>() * 1000),
                    UsedMemory = json["mem"]!["rss"]!.Value<long>()
                };
            },
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public async Task<string> GetVersionAsync()
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync("info", cancellationToken),
            async (response, cancellationToken) => JObject.Parse(await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken))["build"]!["version"]!.Value<string>()!,
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public async Task<Stats> GetStatisticsAsync()
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync("stats", cancellationToken),
            ReadJsonAsync<Stats>,
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public async Task<byte[]> CreateWithoutAccidentImage(WithoutAccidentRequestData request)
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.PostAsJsonAsync("image/without-accident", request, cancellationToken),
            (response, cancellationToken) => response.Content.ReadAsByteArrayAsync(cancellationToken: cancellationToken)!,
            timeout: System.Threading.Timeout.InfiniteTimeSpan
        );
    }

    public async Task<byte[]> CreatePointsImageAsync(PointsImageRequest imageRequest)
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.PostAsJsonAsync("image/points", imageRequest, cancellationToken),
            (response, cancellationToken) => response.Content.ReadAsByteArrayAsync(cancellationToken: cancellationToken)!,
            timeout: System.Threading.Timeout.InfiniteTimeSpan
        );
    }

    public async Task<List<byte[]>> CreatePeepoAngryAsync(List<byte[]> avatarFrames)
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.PostAsJsonAsync("image/peepo/angry", avatarFrames, cancellationToken),
            ReadJsonAsync<List<byte[]>>,
            timeout: System.Threading.Timeout.InfiniteTimeSpan
        );
    }

    public async Task<List<byte[]>> CreatePeepoLoveAsync(List<byte[]> avatarFrames)
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.PostAsJsonAsync("image/peepo/love", avatarFrames, cancellationToken),
            ReadJsonAsync<List<byte[]>>,
            timeout: System.Threading.Timeout.InfiniteTimeSpan
        );
    }
}
