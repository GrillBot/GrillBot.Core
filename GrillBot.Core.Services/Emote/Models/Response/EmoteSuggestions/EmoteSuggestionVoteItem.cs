namespace GrillBot.Core.Services.Emote.Models.Response.EmoteSuggestions;

public record EmoteSuggestionVoteItem(
    string UserId,
    bool IsApproved,
    DateTime VotedAtUtc
);
