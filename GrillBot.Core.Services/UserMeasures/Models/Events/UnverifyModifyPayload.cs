namespace GrillBot.Core.Services.UserMeasures.Models.Events;

public class UnverifyModifyPayload
{
    public long LogSetId { get; set; }
    public DateTime? NewEndUtc { get; set; }

    public UnverifyModifyPayload()
    {
    }

    public UnverifyModifyPayload(long logSetId, DateTime? newEndUtc)
    {
        LogSetId = logSetId;
        NewEndUtc = newEndUtc;
    }
}
