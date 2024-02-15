namespace GrillBot.Core.Services.PointsService.Models.Events;

public class CreateTransactionPayload : CreateTransactionBasePayload
{
    public const string QueueName = "points:create_transaction_requests";

    public DateTime CreatedAtUtc { get; set; }
    public string ChannelId { get; set; } = null!;
    public MessageInfo Message { get; set; } = null!;
    public ReactionInfo? Reaction { get; set; }
}
