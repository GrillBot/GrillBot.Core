using GrillBot.Core.Services.PointsService.Models.Users;

namespace GrillBot.Core.Services.PointsService.Models;

public class SynchronizationRequest
{
    public string GuildId { get; set; } = null!;

    public List<ChannelInfo> Channels { get; set; } = new();

    public List<UserInfo> Users { get; set; } = new();
}
