using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.Common.Attributes;
using GrillBot.Core.Services.Graphics.Models.Chart;
using Refit;

namespace GrillBot.Core.Services.Graphics;

[Service("Graphics")]
public interface IGraphicsClient : IServiceClient
{
    [Post("/chart")]
    Task<Stream> CreateChartAsync(ChartRequestData request, CancellationToken cancellationToken = default);
}
