using GrillBot.Core.RabbitMQ;
using GrillBot.Core.Services.SearchingService.Models.Events.Users;

namespace GrillBot.Core.Services.SearchingService.Models.Events;

public class SynchronizationPayload : IPayload
{
    public string QueueName => "searching:synchronization";

    public List<UserSynchronizationItem> Users { get; set; } = new();

    public SynchronizationPayload()
    {
    }

    public SynchronizationPayload(IEnumerable<UserSynchronizationItem> users)
    {
        Users = users.ToList();
    }
}
