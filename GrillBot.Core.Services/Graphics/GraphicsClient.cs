using GrillBot.Core.Managers.Performance;
using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.Common.Extensions;
using GrillBot.Core.Services.Diagnostics.Models;
using GrillBot.Core.Services.Graphics.Models.Chart;
using GrillBot.Core.Services.Graphics.Models.Diagnostics;
using GrillBot.Core.Services.Graphics.Models.Images;

namespace GrillBot.Core.Services.Graphics;

public class GraphicsClient : RestServiceBase, IGraphicsClient
{
    private static readonly TimeSpan _defaultTimeout = Timeout.InfiniteTimeSpan;

    public override string ServiceName => "Graphics";

    public GraphicsClient(IHttpClientFactory httpClientFactory, ICounterManager counterManager) : base(counterManager, httpClientFactory)
    {
    }

    public async Task<byte[]> CreateChartAsync(ChartRequestData request)
        => await ProcessRequestWithFileAsync(() => HttpMethod.Post.ToRequest("chart", request), _defaultTimeout);

    public override async Task<DiagnosticInfo> GetDiagnosticAsync()
    {
        var timeout = TimeSpan.FromSeconds(30);
        var metrics = (await ProcessRequestAsync<Metrics>(() => HttpMethod.Get.ToRequest("metrics"), timeout))!;
        var stats = (await ProcessRequestAsync<Stats>(() => HttpMethod.Get.ToRequest("stats"), timeout))!;

        return new DiagnosticInfo
        {
            CpuTime = stats.CpuTime,
            DatabaseStatistics = null,
            Endpoints = stats.Endpoints,
            MeasuredFrom = stats.MeasuredFrom,
            Operations = new(),
            RequestsCount = stats.RequestsCount,
            Uptime = (long)Math.Ceiling(metrics.Uptime * 1000),
            UsedMemory = metrics.Mem.Rss
        };
    }

    public async Task<byte[]> CreateWithoutAccidentImage(WithoutAccidentRequestData request)
        => await ProcessRequestWithFileAsync(() => HttpMethod.Post.ToRequest("image/without-accident", request), _defaultTimeout);

    public async Task<byte[]> CreatePointsImageAsync(PointsImageRequest imageRequest)
        => await ProcessRequestWithFileAsync(() => HttpMethod.Post.ToRequest("image/points", imageRequest), _defaultTimeout);

    public async Task<List<byte[]>> CreatePeepoAngryAsync(List<byte[]> avatarFrames)
        => (await ProcessRequestAsync<List<byte[]>>(() => HttpMethod.Post.ToRequest("image/peepo/angry", avatarFrames), _defaultTimeout))!;

    public async Task<List<byte[]>> CreatePeepoLoveAsync(List<byte[]> avatarFrames)
        => (await ProcessRequestAsync<List<byte[]>>(() => HttpMethod.Post.ToRequest("image/peepo/love", avatarFrames), _defaultTimeout))!;
}
