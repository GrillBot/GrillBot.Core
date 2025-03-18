namespace GrillBot.Core.Services.RemindService.Models.Events;

public class SendRemindNotificationPayload
{
    public long RemindId { get; set; }
    public bool IsEarly { get; set; }

    public SendRemindNotificationPayload()
    {
    }

    public SendRemindNotificationPayload(long remindId, bool isEarly)
    {
        RemindId = remindId;
        IsEarly = isEarly;
    }
}
