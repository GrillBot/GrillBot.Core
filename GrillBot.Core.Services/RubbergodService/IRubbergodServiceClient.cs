using GrillBot.Core.Models.Pagination;
using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.Common.Attributes;
using GrillBot.Core.Services.RubbergodService.Models.Help;
using GrillBot.Core.Services.RubbergodService.Models.Karma;
using Refit;

namespace GrillBot.Core.Services.RubbergodService;

[Service("RubbergodService")]
public interface IRubbergodServiceClient : IServiceClient
{
    [Get("api/karma")]
    Task<PaginatedResponse<UserKarma>> GetKarmaPageAsync([Query] PaginatedParams parameters);

    [Get("api/pins/{guildId}/{channelId}")]
    Task<byte[]> GetPinsAsync(ulong guildId, ulong channelId, [Query] bool markdown);

    [Get("api/help/slashcommands")]
    Task<Dictionary<string, Cog>> GetSlashCommandsAsync();
}
