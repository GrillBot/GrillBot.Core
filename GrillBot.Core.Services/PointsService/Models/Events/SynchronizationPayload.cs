using GrillBot.Core.RabbitMQ;
using GrillBot.Core.Services.PointsService.Models.Channels;
using GrillBot.Core.Services.PointsService.Models.Users;

namespace GrillBot.Core.Services.PointsService.Models.Events;

public class SynchronizationPayload : IPayload
{
    public string QueueName => "points:synchronization";

    public string GuildId { get; set; } = null!;
    public List<ChannelSyncItem> Channels { get; set; } = new();
    public List<UserSyncItem> Users { get; set; } = new();

    public SynchronizationPayload()
    {
    }

    public SynchronizationPayload(string guildId, List<ChannelSyncItem> channels, List<UserSyncItem> users)
    {
        GuildId = guildId;
        Channels = channels;
        Users = users;
    }
}
