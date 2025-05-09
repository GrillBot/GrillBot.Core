using GrillBot.Core.Extensions;
using GrillBot.Core.Infrastructure;
using GrillBot.Core.Models;
using GrillBot.Core.Models.Pagination;
using GrillBot.Core.Validation;

namespace GrillBot.Core.Services.Emote.Models.Request.EmoteSuggestions;

public class EmoteSuggestionsListRequest : IDictionaryObject
{
    [DiscordId]
    public ulong? GuildId { get; set; }

    [DiscordId]
    public ulong? FromUserId { get; set; }
    public DateTime? SuggestedFrom { get; set; }
    public DateTime? SuggestedTo { get; set; }
    public string? NameContains { get; set; }
    public bool? ApprovalState { get; set; }

    public PaginatedParams Pagination { get; set; } = new();

    /// <summary>
    /// Available: SuggestedAt, Name
    /// Default: SuggestedAt
    /// </summary>
    public SortParameters Sort { get; set; } = new() { OrderBy = "SuggestedAt" };

    public Dictionary<string, string?> ToDictionary()
    {
        var result = new Dictionary<string, string?>
        {
            { nameof(GuildId), GuildId?.ToString() },
            { nameof(FromUserId), FromUserId?.ToString() },
            { nameof(SuggestedFrom), SuggestedFrom?.ToString("o") },
            { nameof(SuggestedTo), SuggestedTo?.ToString("o") },
            { nameof(NameContains), NameContains },
            { nameof(ApprovalState), ApprovalState?.ToString() }
        };

        result.MergeDictionaryObjects(Pagination, nameof(Pagination));
        result.MergeDictionaryObjects(Sort, nameof(Sort));

        return result;
    }
}
