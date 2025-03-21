﻿using GrillBot.Core.RabbitMQ.V2.Messages;

namespace GrillBot.Core.Services.RemindService.Models.Events;

public class SendRemindNotificationPayload : IRabbitMessage
{
    public string Topic => "Remind";
    public string Queue => "SendRemindNotification";

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
