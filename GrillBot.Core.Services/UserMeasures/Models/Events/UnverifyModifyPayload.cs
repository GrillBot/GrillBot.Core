using GrillBot.Core.RabbitMQ;

namespace GrillBot.Core.Services.UserMeasures.Models.Events;

public class UnverifyModifyPayload : IPayload
{
    public string QueueName => "user_measures:unverify_modify";

    public long LogSetId { get; set; }

    public DateTime? NewEnd { get; set; }

    public UnverifyModifyPayload()
    {
    }

    public UnverifyModifyPayload(long logSetId, DateTime? newEnd)
    {
        LogSetId = logSetId;
        NewEnd = newEnd;
    }
}
