﻿namespace GrillBot.Core.Services.PointsService.Models.Events;

public class CreateTransactionAdminPayload : CreateTransactionBasePayload
{
    public override string Queue => "CreateTransactionAdmin";

    public string UserId { get; set; } = null!;
    public int Amount { get; set; }

    public CreateTransactionAdminPayload()
    {
    }

    public CreateTransactionAdminPayload(string guildId, string userId, int amount) : base(guildId)
    {
        UserId = userId;
        Amount = amount;
    }
}
