using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.Common.Attributes;
using GrillBot.Core.Services.ImageProcessing.Models;
using Refit;

namespace GrillBot.Core.Services.ImageProcessing;

[Service("ImageProcessing")]
public interface IImageProcessingClient : IServiceClient
{
    [Post("/api/image/peepoLove")]
    Task<byte[]> CreatePeepoloveImageAsync(PeepoRequest request, CancellationToken cancellationToken = default);

    [Post("/api/image/peepoangry")]
    Task<byte[]> CreatePeepoangryImageAsync(PeepoRequest request, CancellationToken cancellationToken = default);

    [Post("/api/image/points")]
    Task<byte[]> CreatePointsImageAsync(PointsRequest request, CancellationToken cancellationToken = default);

    [Post("/api/image/without-accident")]
    Task<byte[]> CreateWithoutAccidentImageAsync(WithoutAccidentImageRequest request, CancellationToken cancellationToken = default);

    [Post("/api/image/chart")]
    Task<byte[]> CreateChartImageAsync(ChartRequest request, CancellationToken cancellationToken = default);
}
