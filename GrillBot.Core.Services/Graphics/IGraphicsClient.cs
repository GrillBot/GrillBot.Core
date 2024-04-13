using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.Graphics.Models.Chart;
using GrillBot.Core.Services.Graphics.Models.Images;

namespace GrillBot.Core.Services.Graphics;

public interface IGraphicsClient : IClient
{
    Task<byte[]> CreateChartAsync(ChartRequestData request);
    Task<byte[]> CreateWithoutAccidentImage(WithoutAccidentRequestData request);
    Task<byte[]> CreatePointsImageAsync(PointsImageRequest imageRequest);
    Task<List<byte[]>> CreatePeepoAngryAsync(List<byte[]> avatarFrames);
    Task<List<byte[]>> CreatePeepoLoveAsync(List<byte[]> avatarFrames);
}
