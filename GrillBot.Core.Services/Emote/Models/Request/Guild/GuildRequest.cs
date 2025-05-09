using GrillBot.Core.Infrastructure;
using GrillBot.Core.Validation;

namespace GrillBot.Core.Services.Emote.Models.Request.Guild;

public class GuildRequest : IDictionaryObject
{
    [DiscordId]
    public ulong? SuggestionChannelId { get; set; }

    [DiscordId]
    public ulong? VoteChannelId { get; set; }

    public TimeSpan VoteTime { get; set; }

    public Dictionary<string, string?> ToDictionary()
    {
        return new Dictionary<string, string?>()
        {
            { nameof(SuggestionChannelId), SuggestionChannelId?.ToString() },
            { nameof(VoteChannelId), VoteChannelId?.ToString() },
            { nameof(VoteTime), VoteTime.ToString("c") }
        };
    }
}
