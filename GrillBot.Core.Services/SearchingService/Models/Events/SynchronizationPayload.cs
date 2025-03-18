using GrillBot.Core.Services.SearchingService.Models.Events.Users;

namespace GrillBot.Core.Services.SearchingService.Models.Events;

public class SynchronizationPayload
{
    public List<UserSynchronizationItem> Users { get; set; } = [];

    public SynchronizationPayload()
    {
    }

    public SynchronizationPayload(IEnumerable<UserSynchronizationItem> users)
    {
        Users = [.. users];
    }
}
