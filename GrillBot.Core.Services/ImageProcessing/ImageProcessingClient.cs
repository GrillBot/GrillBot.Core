using GrillBot.Core.Infrastructure.Auth;
using GrillBot.Core.Managers.Performance;
using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.Common.Extensions;
using GrillBot.Core.Services.ImageProcessing.Models;

namespace GrillBot.Core.Services.ImageProcessing;

public class ImageProcessingClient : RestServiceBase, IImageProcessingClient
{
    private static readonly TimeSpan _defaultTimeout = Timeout.InfiniteTimeSpan;

    public override string ServiceName => "ImageProcessing";

    public ImageProcessingClient(ICounterManager counterManager, IHttpClientFactory httpClientFactory, ICurrentUserProvider currentUser)
        : base(counterManager, httpClientFactory, currentUser) { }

    public async Task<byte[]> CreatePeepoloveImageAsync(PeepoRequest request)
        => await ProcessRequestWithFileAsync(() => HttpMethod.Post.ToRequest("api/image/peepoLove", request), _defaultTimeout);

    public async Task<byte[]> CreatePeepoangryImageAsync(PeepoRequest request)
        => await ProcessRequestWithFileAsync(() => HttpMethod.Post.ToRequest("api/image/peepoangry", request), _defaultTimeout);

    public async Task<byte[]> CreatePointsImageAsync(PointsRequest request)
        => await ProcessRequestWithFileAsync(() => HttpMethod.Post.ToRequest("api/image/points", request), _defaultTimeout);

    public async Task<byte[]> CreateWithoutAccidentImageAsync(WithoutAccidentImageRequest request)
        => await ProcessRequestWithFileAsync(() => HttpMethod.Post.ToRequest("api/image/without-accident", request), _defaultTimeout);

    public async Task<byte[]> CreateChartImageAsync(ChartRequest request)
        => await ProcessRequestWithFileAsync(() => HttpMethod.Post.ToRequest("api/image/chart", request), _defaultTimeout);
}
