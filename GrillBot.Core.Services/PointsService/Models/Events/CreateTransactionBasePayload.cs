namespace GrillBot.Core.Services.PointsService.Models.Events;

public abstract class CreateTransactionBasePayload
{
    public string GuildId { get; set; } = null!;
}
