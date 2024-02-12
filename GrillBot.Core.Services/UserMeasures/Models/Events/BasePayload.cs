namespace GrillBot.Core.Services.UserMeasures.Models.Events;

public class BasePayload
{
    public DateTime CreatedAt { get; set; }
    public string Reason { get; set; } = null!;
    public string GuildId { get; set; } = null!;
    public string ModeratorId { get; set; } = null!;
    public string TargetUserId { get; set; } = null!;
}
