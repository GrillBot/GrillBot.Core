using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.Diagnostics.Models;
using GrillBot.Core.Services.ImageProcessing.Models;

namespace GrillBot.Core.Services.ImageProcessing;

public interface IImageProcessingClient : IClient
{
    Task<DiagnosticInfo> GetDiagAsync();
    Task<byte[]> CreatePeepoloveImageAsync(PeepoRequest request);
    Task<byte[]> CreatePeepoangryImageAsync(PeepoRequest request);
    Task<byte[]> CreatePointsImageAsync(PointsRequest request);
    Task<byte[]> CreateWithoutAccidentImageAsync(WithoutAccidentImageRequest request);
    Task<byte[]> CreateChartImageAsync(ChartRequest request);
}
