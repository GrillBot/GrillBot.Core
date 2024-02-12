namespace GrillBot.Core.Services.UserMeasures.Models.Events;

public class DeletePayload
{
    public const string QueueName = "user_measures:delete";

    public Guid Id { get; set; }
}
