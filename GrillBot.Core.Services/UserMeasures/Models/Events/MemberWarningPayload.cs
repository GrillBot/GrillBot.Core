using GrillBot.Core.Services.UserMeasures.Models.Events;

namespace GrillBot.Core.Services.UserMeasures.Models.Events;

public class MemberWarningPayload : BasePayload
{
    public const string QueueName = "user_measures:member_warning";
}
