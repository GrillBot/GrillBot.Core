﻿namespace GrillBot.Core.Services.UserMeasures.Models.Events;

public class TimeoutPayload : BasePayload
{
    public override string Queue => "Timeout";

    public long ExternalId { get; set; }
    public DateTime ValidToUtc { get; set; }

    public TimeoutPayload()
    {
    }

    public TimeoutPayload(DateTime createdAtUtc, string reason, string guildId, string moderatorId, string targetUserId, DateTime validToUtc, long externalTimeoutId)
        : base(createdAtUtc, reason, guildId, moderatorId, targetUserId)
    {
        ValidToUtc = validToUtc;
        ExternalId = externalTimeoutId;
    }
}
