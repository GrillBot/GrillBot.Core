namespace GrillBot.Core.Services.Emote.Models.Response.Guild;

public record GuildData(
    ulong? SuggestionChannelId,
    ulong? VoteChannelId,
    TimeSpan VoteTime
);
