using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.ImageProcessing.Models;

namespace GrillBot.Core.Services.ImageProcessing;

public interface IImageProcessingClient : IClient
{
    Task<byte[]> CreatePeepoloveImageAsync(PeepoRequest request);
    Task<byte[]> CreatePeepoangryImageAsync(PeepoRequest request);
    Task<byte[]> CreatePointsImageAsync(PointsRequest request);
    Task<byte[]> CreateWithoutAccidentImageAsync(WithoutAccidentImageRequest request);
    Task<byte[]> CreateChartImageAsync(ChartRequest request);
}
