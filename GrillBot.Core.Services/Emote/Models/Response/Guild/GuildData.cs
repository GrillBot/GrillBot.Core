namespace GrillBot.Core.Services.Emote.Models.Response.Guild;

public record GuildData(
    string? SuggestionChannelId,
    string? VoteChannelId,
    TimeSpan VoteTime
);
