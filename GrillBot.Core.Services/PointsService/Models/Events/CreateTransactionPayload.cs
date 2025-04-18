﻿namespace GrillBot.Core.Services.PointsService.Models.Events;

public class CreateTransactionPayload : CreateTransactionBasePayload
{
    public override string Queue => "CreateTransaction";

    public DateTime CreatedAtUtc { get; set; }
    public string ChannelId { get; set; } = null!;
    public MessageInfo Message { get; set; } = null!;
    public ReactionInfo? Reaction { get; set; }

    public CreateTransactionPayload()
    {
    }

    public CreateTransactionPayload(string guildId, DateTime createdAtUtc, string channelId, MessageInfo message, ReactionInfo? reaction = null)
        : base(guildId)
    {
        CreatedAtUtc = createdAtUtc;
        ChannelId = channelId;
        Message = message;
        Reaction = reaction;
    }
}
