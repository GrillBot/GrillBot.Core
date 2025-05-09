namespace GrillBot.Core.Services.Emote.Models.Response.EmoteSuggestions;

public record EmoteSuggestionVoteItem(
    ulong UserId,
    bool IsApproved,
    DateTime VotedAtUtc
);
