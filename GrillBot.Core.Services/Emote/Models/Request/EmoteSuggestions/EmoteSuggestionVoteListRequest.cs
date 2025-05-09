using GrillBot.Core.Extensions;
using GrillBot.Core.Infrastructure;
using GrillBot.Core.Models;
using GrillBot.Core.Models.Pagination;
using GrillBot.Core.Validation;

namespace GrillBot.Core.Services.Emote.Models.Request.EmoteSuggestions;

public class EmoteSuggestionVoteListRequest : IDictionaryObject
{
    [DiscordId]
    public ulong? UserId { get; set; }

    public PaginatedParams Pagination { get; set; } = new();
    public SortParameters Sort { get; set; } = new();

    public Dictionary<string, string?> ToDictionary()
    {
        var result = new Dictionary<string, string?>
        {
            { nameof(UserId), UserId?.ToString() }
        };

        result.MergeDictionaryObjects(Pagination, nameof(Pagination));
        result.MergeDictionaryObjects(Sort, nameof(Sort));

        return result;
    }
}
