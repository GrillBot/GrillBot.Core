﻿using GrillBot.Core.Services.UserMeasures.Models.Events;

namespace GrillBot.Core.Services.UserMeasures.Models.Events;

public class UnverifyPayload : BasePayload
{
    public const string QueueName = "user_measures:unverify";

    public DateTime EndAt { get; set; }
    public long LogSetId { get; set; }
}
