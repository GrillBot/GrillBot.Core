using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.Common.Attributes;
using GrillBot.Core.Services.Graphics.Models.Chart;
using GrillBot.Core.Services.Graphics.Models.Images;
using Refit;

namespace GrillBot.Core.Services.Graphics;

[Service("Graphics")]
public interface IGraphicsClient : IServiceClient
{
    [Post("chart")]
    Task<byte[]> CreateChartAsync(ChartRequestData request);

    [Post("image/without-accident")]
    Task<byte[]> CreateWithoutAccidentImage(WithoutAccidentRequestData request);

    [Post("image/points")]
    Task<byte[]> CreatePointsImageAsync(PointsImageRequest imageRequest);

    [Post("image/peepo/angry")]
    Task<List<byte[]>> CreatePeepoAngryAsync(List<byte[]> avatarFrames);

    [Post("image/peepo/love")]
    Task<List<byte[]>> CreatePeepoLoveAsync(List<byte[]> avatarFrames);
}
