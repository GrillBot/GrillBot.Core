namespace GrillBot.Core.Services.UserMeasures.Models.Events;

public class UnverifyModifyPayload
{
    public const string QueueName = "user_measures:unverify_modify";

    public long LogSetId { get; set; }

    public DateTime? NewEnd { get; set; }
}
