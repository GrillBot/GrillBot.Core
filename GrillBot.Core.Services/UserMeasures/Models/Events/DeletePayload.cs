using GrillBot.Core.RabbitMQ;

namespace GrillBot.Core.Services.UserMeasures.Models.Events;

public class DeletePayload : IPayload
{
    public string QueueName => "user_measures:delete";

    public Guid Id { get; set; }

    public DeletePayload()
    {
    }

    public DeletePayload(Guid id)
    {
        Id = id;
    }
}
