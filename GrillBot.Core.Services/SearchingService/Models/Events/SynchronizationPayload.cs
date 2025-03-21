using GrillBot.Core.RabbitMQ.V2.Messages;
using GrillBot.Core.Services.SearchingService.Models.Events.Users;

namespace GrillBot.Core.Services.SearchingService.Models.Events;

public class SynchronizationPayload : IRabbitMessage
{
    public string Topic => "Searching";
    public string Queue => "Synchronization";

    public List<UserSynchronizationItem> Users { get; set; } = [];

    public SynchronizationPayload()
    {
    }

    public SynchronizationPayload(IEnumerable<UserSynchronizationItem> users)
    {
        Users = [.. users];
    }
}
