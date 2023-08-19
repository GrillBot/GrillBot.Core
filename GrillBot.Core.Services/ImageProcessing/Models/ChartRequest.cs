using GrillBot.Core.Services.Graphics.Models.Chart;

namespace GrillBot.Core.Services.ImageProcessing.Models;

public class ChartRequest
{
    public List<ChartRequestData> Requests { get; set; } = new();
}
